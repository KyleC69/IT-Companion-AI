using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(Sample.Metadata))]
    public partial class Sample
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object SampleRunId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SampleUid { get; set; }
    
            [StringLength(1000)]
            public object FeatureUid { get; set; }
    
            [StringLength(200)]
            public object Language { get; set; }
    
            public object Code { get; set; }
    
            [StringLength(400)]
            public object EntryPoint { get; set; }
    
            [StringLength(200)]
            public object TargetFramework { get; set; }
    
            public object PackageReferences { get; set; }
    
            [StringLength(1000)]
            public object DerivedFromCodeUid { get; set; }
    
            public object Tags { get; set; }
    
            public object SampleRun { get; set; }
    
            public object SampleApiMemberLinks { get; set; }
        }
    }
}
