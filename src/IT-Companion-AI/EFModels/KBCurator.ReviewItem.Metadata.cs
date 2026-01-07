// Project Name: SKAgent
// File Name: KBCurator.ReviewItem.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class ReviewItem
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object ReviewRunId { get; set; }

        [StringLength(50)] [Required] public object TargetKind { get; set; }

        [StringLength(1000)] [Required] public object TargetUid { get; set; }

        [StringLength(50)] public object Status { get; set; }

        public object Summary { get; set; }
    }
}