// Project Name: SKAgent
// File Name: KBCurator.ApiFeature.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class ApiFeature
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        public object ApiTypeId { get; set; }

        [StringLength(1000)] [Required] public object SemanticUid { get; set; }

        [Required] public object TruthRunId { get; set; }

        [StringLength(400)] public object Name { get; set; }

        [StringLength(200)] public object Language { get; set; }

        public object Description { get; set; }

        public object Tags { get; set; }

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

        public object ApiMember { get; set; }
    }
}