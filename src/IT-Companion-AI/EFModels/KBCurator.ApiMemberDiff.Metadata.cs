using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ApiMemberDiff.Metadata))]
    public partial class ApiMemberDiff
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
            public object MemberUid { get; set; }
    
            [StringLength(200)]
            public object ChangeKind { get; set; }
    
            public object OldSignature { get; set; }
    
            public object NewSignature { get; set; }
    
            public object Breaking { get; set; }
    
            public object DetailJson { get; set; }
    
            public object SnapshotDiff { get; set; }
        }
    }
}
