using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ReviewIssue.Metadata))]
    public partial class ReviewIssue
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object ReviewItemId { get; set; }
    
            [StringLength(200)]
            public object Code { get; set; }
    
            [StringLength(50)]
            public object Severity { get; set; }
    
            [StringLength(1000)]
            public object RelatedMemberUid { get; set; }
    
            public object Details { get; set; }
    
            public object ReviewItem { get; set; }
        }
    }
}
