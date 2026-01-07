using System.ComponentModel.DataAnnotations;

namespace ITCompanionAI.Entities
{
    [MetadataType(typeof(ApiType.Metadata))]
    public partial class ApiType
    {
        public partial class Metadata
        {
    
            [Key]
            [Required()]
            public object Id { get; set; }
    
            [StringLength(1000)]
            [Required()]
            public object SemanticUid { get; set; }
    
            [Required()]
            public object SourceSnapshotId { get; set; }
    
            [StringLength(400)]
            public object Name { get; set; }
    
            [StringLength(1000)]
            public object NamespacePath { get; set; }
    
            [StringLength(200)]
            public object Kind { get; set; }
    
            [StringLength(200)]
            public object Accessibility { get; set; }
    
            public object IsStatic { get; set; }
    
            public object IsGeneric { get; set; }
    
            public object IsAbstract { get; set; }
    
            public object IsSealed { get; set; }
    
            public object IsRecord { get; set; }
    
            public object IsRefLike { get; set; }
    
            [StringLength(1000)]
            public object BaseTypeUid { get; set; }
    
            public object Interfaces { get; set; }
    
            [StringLength(1000)]
            public object ContainingTypeUid { get; set; }
    
            public object GenericParameters { get; set; }
    
            public object GenericConstraints { get; set; }
    
            public object Summary { get; set; }
    
            public object Remarks { get; set; }
    
            public object Attributes { get; set; }
    
            public object SourceFilePath { get; set; }
    
            public object SourceStartLine { get; set; }
    
            public object SourceEndLine { get; set; }
    
            [Required()]
            public object VersionNumber { get; set; }
    
            [Required()]
            public object CreatedIngestionRunId { get; set; }
    
            [Required()]
            public object UpdatedIngestionRunId { get; set; }
    
            public object RemovedIngestionRunId { get; set; }
    
            [Required()]
            public object ValidFromUtc { get; set; }
    
            public object ValidToUtc { get; set; }
    
            [Required()]
            public object IsActive { get; set; }
    
            public object ContentHash { get; set; }
    
            public object SemanticUidHash { get; set; }
    
            public object SourceSnapshot { get; set; }
    
            public object IngestionRun_CreatedIngestionRunId { get; set; }
    
            public object IngestionRun_UpdatedIngestionRunId { get; set; }
    
            public object IngestionRun_RemovedIngestionRunId { get; set; }
        }
    }
}
