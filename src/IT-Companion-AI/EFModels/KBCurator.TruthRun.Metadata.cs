using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(TruthRun.Metadata))]
    public partial class TruthRun
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
    
            public object ApiFeatures { get; set; }
    
            public object SourceSnapshot { get; set; }
        }
    }
}
