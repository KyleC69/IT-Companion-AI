namespace ITCompanionAI.Ingestion;





public class IngestionSettings
{
    public string ModelOnnxPath { get; set; } = "http://localhost:11434";
    public string TokenizerPath { get; set; }
    public string VocabPath { get; set; }
    public string KnowledgeBaseConnectionString { get; set; }
    public string OllamaBaseUrl { get; set; } = "http://localhost:11434";
    public string OllamaModel { get; set; } = "bge-large:latest";
}