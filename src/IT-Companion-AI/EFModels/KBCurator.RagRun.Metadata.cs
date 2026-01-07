using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(RagRun.Metadata))]
    public partial class RagRun
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object SnapshotId { get; set; }
    
            [Required()]
            public object TimestampUtc { get; set; }
    
            [StringLength(200)]
            [Required()]
            public object SchemaVersion { get; set; }
    
            public object RagChunks { get; set; }
    
            public object SourceSnapshot { get; set; }
        }
    }
}
