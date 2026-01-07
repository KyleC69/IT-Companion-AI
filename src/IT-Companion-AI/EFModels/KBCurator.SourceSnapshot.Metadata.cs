using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(SourceSnapshot.Metadata))]
    public partial class SourceSnapshot
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [Required()]
            public object IngestionRunId { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SnapshotUid { get; set; }
    
            public object RepoUrl { get; set; }
    
            [StringLength(200)]
            public object Branch { get; set; }
    
            [StringLength(200)]
            public object RepoCommit { get; set; }
    
            [StringLength(200)]
            public object Language { get; set; }
    
            [StringLength(200)]
            public object PackageName { get; set; }
    
            [StringLength(200)]
            public object PackageVersion { get; set; }
    
            public object ConfigJson { get; set; }
    
            public object SnapshotUidHash { get; set; }
    
            public object ApiTypes { get; set; }
    
            public object DocPages { get; set; }
    
            public object ExecutionRuns { get; set; }
    
            public object RagRuns { get; set; }
    
            public object ReviewRuns { get; set; }
    
            public object SampleRuns { get; set; }
    
            public object SnapshotDiffs_OldSnapshotId { get; set; }
    
            public object SnapshotDiffs_NewSnapshotId { get; set; }
    
            public object IngestionRun { get; set; }
    
            public object TruthRuns { get; set; }
        }
    }
}
