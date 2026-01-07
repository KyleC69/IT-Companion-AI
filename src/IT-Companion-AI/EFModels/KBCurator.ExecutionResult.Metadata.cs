using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ExecutionResult.Metadata))]
    public partial class ExecutionResult
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object ExecutionRunId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SampleUid { get; set; }
    
            [StringLength(100)]
            public object Status { get; set; }
    
            public object BuildLog { get; set; }
    
            public object RunLog { get; set; }
    
            public object ExceptionJson { get; set; }
    
            public object DurationMs { get; set; }
    
            public object ExecutionRun { get; set; }
        }
    }
}
