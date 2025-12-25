using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
// File: RagStoreTests.cs
// Add to your test project (MSTest). Reference the library project that contains:
// MyCompany.Rag.DeterministicEmbedder, SqliteMetadataStore, HnswVectorIndex, RagStore
//
// NuGet for test project:
// dotnet add package Microsoft.NET.Test.Sdk
// dotnet add package MSTest.TestAdapter
// dotnet add package MSTest.TestFramework

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using VectorLib.Rag;




namespace CompanionTests;




[TestClass]
public class RagStoreTests
{
    private string _tempFolder = null!;
    private string _dbPath = null!;
    private string _vectorsPath = null!;
    private string _graphPath = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        // Create a temp folder per test run to avoid collisions
        _tempFolder = Path.Combine(Path.GetTempPath(), "rag_tests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempFolder);

        _dbPath = Path.Combine(_tempFolder, "rag_test.db");
        _vectorsPath = Path.Combine(_tempFolder, "vectors.msgpack");
        _graphPath = Path.Combine(_tempFolder, "index.graph");
    }

    [TestCleanup]
    public void TestCleanup()
    {
        try
        {
            if (Directory.Exists(_tempFolder))
                Directory.Delete(_tempFolder, recursive: true);
        }
        catch
        {
            // best-effort cleanup; don't fail tests on cleanup errors
        }
    }

    [TestMethod]
    public async Task UpsertAndSearch_ReturnsInsertedDocument()
    {
        // Arrange
        var dim = 128;
        var embedder = new DeterministicEmbedder(dim);
        using var meta = new SqliteMetadataStore(_dbPath);
        using var index = new HnswVectorIndex(dim);
        var rag = new RagStore(embedder, meta, index);

        Assert.AreEqual(dim, embedder.Dimension, "Embedder dimension must match index dimension");

        var docId = "doc-semantic-kernel";
        var docText = "Semantic kernel is an orchestration layer for LLMs and tool invocation.";
        await rag.UpsertAsync(docId, docText, source: "unit-test");

        var storedDoc = await meta.GetDocumentAsync(docId);
        Assert.IsNotNull(storedDoc, "Document should be persisted before querying");
        Assert.AreEqual(docText, storedDoc!.Content, "Stored document content should match input");

        // Act
        var results = await rag.SearchAsync("what is semantic kernel?", k: 5);

        // Assert
        Assert.IsNotNull(results);
        Assert.IsTrue(results.Count > 0, "Expected at least one search result.");
        Assert.IsTrue(results.Count <= 5, "Result count should respect k parameter.");
        var found = results.Any(r => string.Equals(r.Id, docId, StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(found, $"Expected search results to contain document id '{docId}'.");

        // Cleanup
        rag.SaveIndex(_vectorsPath, _graphPath);
        Assert.IsTrue(File.Exists(_vectorsPath), "Vector snapshot file should exist after save.");
        Assert.IsTrue(new FileInfo(_vectorsPath).Length > 0, "Vector snapshot should contain data.");
        Assert.IsTrue(File.Exists(_graphPath), "Graph snapshot file should exist after save.");
        Assert.IsTrue(new FileInfo(_graphPath).Length > 0, "Graph snapshot should contain data.");
        rag.Dispose();
    }

    [TestMethod]
    public async Task SaveAndLoad_PersistsIndexAndMetadata()
    {
        // Arrange - create and seed a store, then save index + metadata
        var dim = 128;
        var embedder = new DeterministicEmbedder(dim);

        // Create first store and seed
        using (var meta1 = new SqliteMetadataStore(_dbPath))
        using (var index1 = new HnswVectorIndex(dim))
        {
            var rag1 = new RagStore(embedder, meta1, index1);
            await rag1.UpsertAsync("doc-1", "This document explains the semantic kernel concept.", "seed");
            await rag1.UpsertAsync("doc-2", "Another document about retrieval augmented generation.", "seed");

            var seededDocs = await meta1.GetAllDocumentsAsync();
            Assert.AreEqual(2, seededDocs.Count(), "Expected metadata store to contain two documents.");

            // Persist metadata is already in SQLite; persist index to disk
            rag1.SaveIndex(_vectorsPath, _graphPath);
            rag1.Dispose();
        }

        Assert.IsTrue(File.Exists(_vectorsPath), "Vectors snapshot missing after save.");
        Assert.IsTrue(File.Exists(_graphPath), "Graph snapshot missing after save.");

        // Act - create a new index instance and metadata store pointing to same files
        using var meta2 = new SqliteMetadataStore(_dbPath);
        using var index2 = new HnswVectorIndex(dim);

        // Load index from disk (vectors + graph). The HnswVectorIndex implementation is expected
        // to expose Load(vectorsPath, graphPath) that reconstructs the in-memory index.
        index2.Load(_vectorsPath, _graphPath);

        var rag2 = new RagStore(embedder, meta2, index2);

        var doc1 = await meta2.GetDocumentAsync("doc-1");
        Assert.IsNotNull(doc1, "Metadata store should still contain doc-1 after reload.");

        // Query for a phrase that should match doc-1
        var results = await rag2.SearchAsync("explain semantic kernel", k: 5);

        // Assert
        Assert.IsNotNull(results);
        Assert.IsTrue(results.Count > 0, "Expected at least one search result after load.");
        var found = results.Any(r => string.Equals(r.Id, "doc-1", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(found, "Expected loaded index to return the previously inserted document 'doc-1'.");

        rag2.Dispose();
    }

    [TestMethod]
    public async Task MultipleUpserts_DoNotThrow_AndReturnTopK()
    {
        var dim = 128;
        var embedder = new DeterministicEmbedder(dim);
        using var meta = new SqliteMetadataStore(_dbPath);
        using var index = new HnswVectorIndex(dim);
        var rag = new RagStore(embedder, meta, index);

        // Insert multiple docs
        for (int i = 0; i < 20; i++)
        {
            var id = $"doc-{i}";
            var content = $"Sample document number {i} about topic {(i % 3 == 0 ? "semantic kernel" : "other")}";
            await rag.UpsertAsync(id, content, "bulk-test");
        }

        var allDocs = await meta.GetAllDocumentsAsync();
        Assert.AreEqual(20, allDocs.Count(), "All documents should be persisted to metadata store.");

        // Query
        var results = await rag.SearchAsync("semantic kernel explanation", k: 10);

        Assert.IsNotNull(results);
        Assert.IsTrue(results.Count > 0, "Expected results for multi-upsert query.");
        Assert.IsTrue(results.Count <= 10, "Result set should respect requested top-k size.");
        // At least one result should contain "semantic kernel" in content
        var anySemantic = results.Any(r => r.Content?.IndexOf("semantic kernel", StringComparison.OrdinalIgnoreCase) >= 0);
        Assert.IsTrue(anySemantic, "Expected at least one returned document to mention 'semantic kernel'.");

        rag.Dispose();
    }
}
    