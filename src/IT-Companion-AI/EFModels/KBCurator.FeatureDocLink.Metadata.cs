using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(FeatureDocLink.Metadata))]
    public partial class FeatureDocLink
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
            public object DocUid { get; set; }
    
            [StringLength(1000)]
            public object SectionUid { get; set; }
    
            public object ApiFeature { get; set; }
        }
    }
}
