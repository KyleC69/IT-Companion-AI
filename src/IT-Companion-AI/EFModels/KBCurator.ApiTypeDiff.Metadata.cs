using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ApiTypeDiff.Metadata))]
    public partial class ApiTypeDiff
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object SnapshotDiffId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object TypeUid { get; set; }
    
            [StringLength(200)]
            public object ChangeKind { get; set; }
    
            public object DetailJson { get; set; }
    
            public object SnapshotDiff { get; set; }
        }
    }
}
