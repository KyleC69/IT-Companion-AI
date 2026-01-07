// Project Name: SKAgent
// File Name: KBCurator.SourceSnapshot.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class SourceSnapshot
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object IngestionRunId { get; set; }

        [StringLength(200)] [Required] public object SnapshotUid { get; set; }

        public object RepoUrl { get; set; }

        [StringLength(200)] public object Branch { get; set; }

        [StringLength(200)] public object RepoCommit { get; set; }

        [StringLength(200)] public object Language { get; set; }

        [StringLength(200)] public object PackageName { get; set; }

        [StringLength(200)] public object PackageVersion { get; set; }

        public object ConfigJson { get; set; }

        public object SnapshotUidHash { get; set; }
    }
}