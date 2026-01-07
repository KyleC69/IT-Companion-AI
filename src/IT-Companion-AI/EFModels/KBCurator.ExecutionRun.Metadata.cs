using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ExecutionRun.Metadata))]
    public partial class ExecutionRun
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object SnapshotId { get; set; }
    
            [Required()]
            public object SampleRunId { get; set; }
    
            [Required()]
            public object TimestampUtc { get; set; }
    
            public object EnvironmentJson { get; set; }
    
            [StringLength(200)]
            [Required()]
            public object SchemaVersion { get; set; }
    
            public object ExecutionResults { get; set; }
    
            public object SourceSnapshot { get; set; }
    
            public object SampleRun { get; set; }
        }
    }
}
