using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(FeatureMemberLink.Metadata))]
    public partial class FeatureMemberLink
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object FeatureId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object MemberUid { get; set; }
    
            [StringLength(50)]
            public object Role { get; set; }
    
            public object ApiFeature { get; set; }
        }
    }
}
