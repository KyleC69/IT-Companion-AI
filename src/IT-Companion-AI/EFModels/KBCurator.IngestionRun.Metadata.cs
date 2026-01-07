using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(IngestionRun.Metadata))]
    public partial class IngestionRun
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object TimestampUtc { get; set; }
    
            [StringLength(200)]
            [Required()]
            public object SchemaVersion { get; set; }
    
            public object Notes { get; set; }
    
            public object ApiFeatures_CreatedIngestionRunId { get; set; }
    
            public object ApiFeatures_UpdatedIngestionRunId { get; set; }
    
            public object ApiFeatures_RemovedIngestionRunId { get; set; }
    
            public object ApiMembers_CreatedIngestionRunId { get; set; }
    
            public object ApiMembers_UpdatedIngestionRunId { get; set; }
    
            public object ApiMembers_RemovedIngestionRunId { get; set; }
    
            public object ApiTypes_CreatedIngestionRunId { get; set; }
    
            public object ApiTypes_UpdatedIngestionRunId { get; set; }
    
            public object ApiTypes_RemovedIngestionRunId { get; set; }
    
            public object CodeBlocks_CreatedIngestionRunId { get; set; }
    
            public object CodeBlocks_UpdatedIngestionRunId { get; set; }
    
            public object CodeBlocks_RemovedIngestionRunId { get; set; }
    
            public object DocPages_CreatedIngestionRunId { get; set; }
    
            public object DocPages_UpdatedIngestionRunId { get; set; }
    
            public object DocPages_RemovedIngestionRunId { get; set; }
    
            public object DocSections_CreatedIngestionRunId { get; set; }
    
            public object DocSections_UpdatedIngestionRunId { get; set; }
    
            public object DocSections_RemovedIngestionRunId { get; set; }
    
            public object SourceSnapshots { get; set; }
        }
    }
}
