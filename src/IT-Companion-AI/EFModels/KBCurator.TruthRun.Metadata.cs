// Project Name: SKAgent
// File Name: KBCurator.TruthRun.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class TruthRun
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object SnapshotId { get; set; }

        [Required] public object TimestampUtc { get; set; }

        [StringLength(200)] [Required] public object SchemaVersion { get; set; }
    }
}