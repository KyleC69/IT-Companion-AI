using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(CodeBlock.Metadata))]
    public partial class CodeBlock
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object DocSectionId { get; set; }
    
            [StringLength(1000)]
            public object SemanticUid { get; set; }
    
            [StringLength(200)]
            public object Language { get; set; }
    
            public object Content { get; set; }
    
            public object DeclaredPackages { get; set; }
    
            public object Tags { get; set; }
    
            public object InlineComments { get; set; }
    
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
    
            public object DocSection { get; set; }
    
            public object IngestionRun_CreatedIngestionRunId { get; set; }
    
            public object IngestionRun_UpdatedIngestionRunId { get; set; }
    
            public object IngestionRun_RemovedIngestionRunId { get; set; }
        }
    }
}
