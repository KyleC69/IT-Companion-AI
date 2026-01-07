using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(VApiFeatureCurrent.Metadata))]
    public partial class VApiFeatureCurrent
    {
        public partial class Metadata
        {
    
            [Required()]
            public object Id { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SemanticUid { get; set; }
    
            [Required()]
            public object TruthRunId { get; set; }
    
            [StringLength(400)]
            public object Name { get; set; }
    
            [StringLength(200)]
            public object Language { get; set; }
    
            public object Description { get; set; }
    
            public object Tags { get; set; }
    
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
        }
    }
}
