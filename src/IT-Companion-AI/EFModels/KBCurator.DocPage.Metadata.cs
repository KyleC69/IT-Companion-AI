// Project Name: SKAgent
// File Name: KBCurator.DocPage.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class DocPage
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [StringLength(1000)] [Required] public object SemanticUid { get; set; }

        [Required] public object SourceSnapshotId { get; set; }

        public object SourcePath { get; set; }

        [StringLength(400)] public object Title { get; set; }

        [StringLength(200)] public object Language { get; set; }

        public object Url { get; set; }

        public object RawMarkdown { get; set; }

        [Required] public object VersionNumber { get; set; }

        [Required] public object CreatedIngestionRunId { get; set; }

        [Required] public object UpdatedIngestionRunId { get; set; }

        public object RemovedIngestionRunId { get; set; }

        [Required] public object ValidFromUtc { get; set; }

        public object ValidToUtc { get; set; }

        [Required] public object IsActive { get; set; }

        public object ContentHash { get; set; }

        public object SemanticUidHash { get; set; }

        public object IngestionRun { get; set; }
    }
}