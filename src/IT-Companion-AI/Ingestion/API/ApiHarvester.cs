using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;


namespace ITCompanionAI.AgentFramework.Ingestion;

public class ApiHarvester: RoslynHarvesterBase
{
    
    private KBContext _db= new KBContext();
    private Guid? SessionID = Guid.Empty;
    
    
    public ApiHarvester(string filePath) 
    {
    }







    public async Task ExtractAsync(string filePath,CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        
        //Temporary hardcoded path for testing
      

        var solution =await LoadSolutionFromDirectoryAsync(filePath,cancellationToken);
        
        
        ArgumentNullException.ThrowIfNull(solution);

        //Create instance of walker ApiExtractionWalker
        var walker = new ApiExtractionOrchestrator();
        //Execute walker on each document in solution
 var results=     await  walker.ExtractAsync(solution, cancellationToken);
 

 
 
 
 
        List<ApiType> extractedTypes = new List<ApiType>();
        
        foreach (var type in results.Types)
        {
            var k = new ApiType()
            {
                Id = Guid.NewGuid(),
                SemanticUid = type.SemanticUid,
                SourceSnapshotId = default, // This needs to be populated from the ingestion run
                Name = type.Name,
                NamespacePath = type.Namespace,
                Kind = type.Kind,
                Accessibility = type.Accessibility,
                IsStatic = type.Modifiers.Contains("static"),
                IsGeneric = type.GenericParameters.Any(),
                IsAbstract = type.Modifiers.Contains("abstract"),
                IsSealed = type.Modifiers.Contains("sealed"),
                IsRecord = type.Kind.Equals("record", StringComparison.OrdinalIgnoreCase),
                IsRefLike = type.Modifiers.Contains("ref struct") || type.Modifiers.Contains("ref readonly struct"),
                BaseTypeUid = type.BaseType,
                Interfaces = string.Join(";", type.Interfaces),
                ContainingTypeUid = type.DeclaringType,
                GenericParameters = string.Join(";", type.GenericParameters),
                GenericConstraints = string.Join(";", type.GenericConstraints),
                Summary = type.SummaryXml, // Assuming SummaryXml can be directly mapped to Summary
                Remarks = null, // No direct mapping for Remarks from ApiTypeExtraction
                Attributes = string.Join(";", type.Attributes),
                SourceFilePath = type.SourceFilePath,
                SourceStartLine = type.StartLine,
                SourceEndLine = type.EndLine,
                VersionNumber = 1, // Default to 1, logic for incrementing may be needed elsewhere
                CreatedIngestionRunId = default, // This needs to be populated from the ingestion run
                UpdatedIngestionRunId = default, // This needs to be populated from the ingestion run
                RemovedIngestionRunId = null,
                ValidFromUtc = DateTime.UtcNow,
                ValidToUtc = null,
                IsActive = true,
                ContentHash = null,// CalculateHash(type), // Needs to be computed if required
                SemanticUidHash = CalculateHash(type.SemanticUid)
            };
            
            extractedTypes.Add(k);
        }


        foreach (var mem in results.Members)
        {
            var m = new ApiMember
            {
                SemanticUid = mem.SemanticUid,
                Id = Guid.NewGuid(),
                ApiFeatureId =
                    default, // This needs to be populated from the ingestion run. Likely derived from the parent ApiType.
                Name = mem.Name,
                Kind = mem.Kind,
                MethodKind = null, // Not directly available from ApiMemberExtraction
                Accessibility = mem.Accessibility,
                IsStatic = mem.Modifiers.Contains("static"),
                IsExtensionMethod = mem.Modifiers.Contains("extension"), // Assuming 'extension' is a modifier
                IsAsync = mem.Modifiers.Contains("async"),
                IsVirtual = mem.Modifiers.Contains("virtual"),
                IsOverride = mem.Modifiers.Contains("override"),
                IsAbstract = mem.Modifiers.Contains("abstract"),
                IsSealed = mem.Modifiers.Contains("sealed"),
                IsReadonly = mem.Modifiers.Contains("readonly"),
                IsConst = mem.Modifiers.Contains("const"),
                IsUnsafe = mem.Modifiers.Contains("unsafe"),
                ReturnTypeUid = mem.ReturnType, // Assuming ReturnType maps directly to ReturnTypeUid
                ReturnNullable = null, // Not directly available from ApiMemberExtraction
                GenericParameters = string.Join(";",
                    mem.GenericParameters),
                GenericConstraints = string.Join(";",
                    mem.GenericConstraints),
                Summary = mem.SummaryXml, // Assuming SummaryXml can be directly mapped to Summary
                Remarks = null, // No direct mapping for Remarks from ApiMemberExtraction
                Attributes = string.Join(";",
                    mem.Attributes),
                SourceFilePath = mem.SourceFilePath,
                SourceStartLine = mem.StartLine,
                SourceEndLine = mem.EndLine,
                MemberUidHash = CalculateHash(mem.SemanticUid), // Needs to be computed
                VersionNumber = 1, // Default to 1, logic for incrementing may be needed elsewhere
                CreatedIngestionRunId = default, // This needs to be populated from the ingestion run
                UpdatedIngestionRunId = default, // This needs to be populated from the ingestion run
                RemovedIngestionRunId = null,
                ValidFromUtc = DateTime.UtcNow,
                ValidToUtc = null,
                IsActive = true,
                ContentHash =
                    CalculateHash(CalculateHash(mem.SemanticUid)
                        .ToString()), // Placeholder: Needs actual content hashing
                SemanticUidHash = CalculateHash(mem.SemanticUid),
                ApiFeature = null,
                IngestionRun = null
            };
        }


    }

    private byte[] CalculateHash(string typeSemanticUid)
    {
        if (string.IsNullOrEmpty(typeSemanticUid)) throw new ArgumentException("The typeSemanticUid cannot be null or empty.", nameof(typeSemanticUid));

        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(typeSemanticUid));
        }
    }
}



