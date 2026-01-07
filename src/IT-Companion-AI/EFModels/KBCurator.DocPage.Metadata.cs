using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(DocPage.Metadata))]
    public partial class DocPage
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SemanticUid { get; set; }
    
            [Required()]
            public object SourceSnapshotId { get; set; }
    
            public object SourcePath { get; set; }
    
            [StringLength(400)]
            public object Title { get; set; }
    
            [StringLength(200)]
            public object Language { get; set; }
    
            public object Url { get; set; }
    
            public object RawMarkdown { get; set; }
    
            [Required()]
            public object VersionNumber { get; set; }
    
            [Required()]
            public object CreatedIngestionRunId { get; set; }
    
            [Required()]
            public object UpdatedIngestionRunId { get; set; }
    
            public object RemovedIngestionRunId { get; set; }
    
            [Required()]
            public object ValidFromUtc { get; set; }
    
            public object ValidToUtc { get; set; }
    
            [Required()]
            public object IsActive { get; set; }
    
            public object ContentHash { get; set; }
    
            public object SemanticUidHash { get; set; }
    
            public object SourceSnapshot { get; set; }
    
            public object IngestionRun_CreatedIngestionRunId { get; set; }
    
            public object IngestionRun_UpdatedIngestionRunId { get; set; }
    
            public object IngestionRun_RemovedIngestionRunId { get; set; }
    
            public object SemanticIdentity { get; set; }
    
            public object DocSections { get; set; }
        }
    }
}
