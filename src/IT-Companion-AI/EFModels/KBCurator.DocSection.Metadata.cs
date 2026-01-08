// Project Name: SKAgent
// File Name: KBCurator.DocSection.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class DocSection
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object DocPageId { get; set; }

        [StringLength(1000)] [Required] public object SemanticUid { get; set; }

        [StringLength(400)] public object Heading { get; set; }

        public object Level { get; set; }

        public object ContentMarkdown { get; set; }

        public object OrderIndex { get; set; }

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