using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(SampleApiMemberLink.Metadata))]
    public partial class SampleApiMemberLink
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object SampleId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object MemberUid { get; set; }
    
            public object Sample { get; set; }
        }
    }
}
