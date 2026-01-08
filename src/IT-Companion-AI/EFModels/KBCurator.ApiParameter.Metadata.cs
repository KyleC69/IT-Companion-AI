// Project Name: SKAgent
// File Name: KBCurator.ApiParameter.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class ApiParameter
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object ApiMemberId { get; set; }

        [StringLength(200)] public object Name { get; set; }

        [StringLength(1000)] public object TypeUid { get; set; }

        [StringLength(50)] public object NullableAnnotation { get; set; }

        public object Position { get; set; }

        [StringLength(50)] public object Modifier { get; set; }

        public object HasDefaultValue { get; set; }

        public object DefaultValueLiteral { get; set; }

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