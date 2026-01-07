using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(SnapshotDiff.Metadata))]
    public partial class SnapshotDiff
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object OldSnapshotId { get; set; }
    
            [Required()]
            public object NewSnapshotId { get; set; }
    
            [Required()]
            public object TimestampUtc { get; set; }
    
            [StringLength(200)]
            [Required()]
            public object SchemaVersion { get; set; }
    
            public object ApiMemberDiffs { get; set; }
    
            public object ApiTypeDiffs { get; set; }
    
            public object DocPageDiffs { get; set; }
    
            public object SourceSnapshot_OldSnapshotId { get; set; }
    
            public object SourceSnapshot_NewSnapshotId { get; set; }
        }
    }
}
