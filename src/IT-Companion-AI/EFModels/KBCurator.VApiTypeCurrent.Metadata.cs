// Project Name: SKAgent
// File Name: KBCurator.VApiTypeCurrent.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class VApiTypeCurrent
{
    public class Metadata
    {
        [Required] public object Id { get; set; }

        [StringLength(1000)] [Required] public object SemanticUid { get; set; }

        [Required] public object SourceSnapshotId { get; set; }

        [StringLength(400)] public object Name { get; set; }

        [StringLength(1000)] public object NamespacePath { get; set; }

        [StringLength(200)] public object Kind { get; set; }

        [StringLength(200)] public object Accessibility { get; set; }

        public object IsStatic { get; set; }

        public object IsGeneric { get; set; }

        public object IsAbstract { get; set; }

        public object IsSealed { get; set; }

        public object IsRecord { get; set; }

        public object IsRefLike { get; set; }

        [StringLength(1000)] public object BaseTypeUid { get; set; }

        public object Interfaces { get; set; }

        [StringLength(1000)] public object ContainingTypeUid { get; set; }

        public object GenericParameters { get; set; }

        public object GenericConstraints { get; set; }

        public object Summary { get; set; }

        public object Remarks { get; set; }

        public object Attributes { get; set; }

        public object SourceFilePath { get; set; }

        public object SourceStartLine { get; set; }

        public object SourceEndLine { get; set; }

        [Required] public object VersionNumber { get; set; }

        [Required] public object CreatedIngestionRunId { get; set; }

        [Required] public object UpdatedIngestionRunId { get; set; }

        public object RemovedIngestionRunId { get; set; }

        [Required] public object ValidFromUtc { get; set; }

        public object ValidToUtc { get; set; }

        [Required] public object IsActive { get; set; }

        public object ContentHash { get; set; }

        public object SemanticUidHash { get; set; }
    }
}