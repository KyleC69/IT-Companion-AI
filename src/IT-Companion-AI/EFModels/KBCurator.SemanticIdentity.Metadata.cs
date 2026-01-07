using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(SemanticIdentity.Metadata))]
    public partial class SemanticIdentity
    {
        public partial class Metadata
        {
    
            [StringLength(1000)]
            [Required()]
            public object Uid { get; set; }
    
            [Key]
            [Required()]
            public object UidHash { get; set; }
    
            [StringLength(50)]
            [Required()]
            public object Kind { get; set; }
    
            [Required()]
            public object CreatedUtc { get; set; }
    
            public object Notes { get; set; }
    
            public object ApiFeatures { get; set; }
    
            public object DocPages { get; set; }
    
            public object DocSections { get; set; }
        }
    }
}
