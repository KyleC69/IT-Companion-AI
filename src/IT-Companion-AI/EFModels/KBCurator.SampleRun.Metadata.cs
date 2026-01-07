using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(SampleRun.Metadata))]
    public partial class SampleRun
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
    
            public object ExecutionRuns { get; set; }
    
            public object Samples { get; set; }
    
            public object SourceSnapshot { get; set; }
        }
    }
}
