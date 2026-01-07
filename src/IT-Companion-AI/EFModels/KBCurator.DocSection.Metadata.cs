using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(DocSection.Metadata))]
    public partial class DocSection
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object DocPageId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SemanticUid { get; set; }
    
            [StringLength(400)]
            public object Heading { get; set; }
    
            public object Level { get; set; }
    
            public object ContentMarkdown { get; set; }
    
            public object OrderIndex { get; set; }
    
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
    
            public object CodeBlocks { get; set; }
    
            public object DocPage { get; set; }
    
            public object IngestionRun_CreatedIngestionRunId { get; set; }
    
            public object IngestionRun_UpdatedIngestionRunId { get; set; }
    
            public object IngestionRun_RemovedIngestionRunId { get; set; }
    
            public object SemanticIdentity { get; set; }
        }
    }
}
