using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ReviewItem.Metadata))]
    public partial class ReviewItem
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object ReviewRunId { get; set; }
    
            [StringLength(50)]
            [Required()]
            public object TargetKind { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object TargetUid { get; set; }
    
            [StringLength(50)]
            public object Status { get; set; }
    
            public object Summary { get; set; }
    
            public object ReviewIssues { get; set; }
    
            public object ReviewRun { get; set; }
        }
    }
}
