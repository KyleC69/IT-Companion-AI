// Project Name: SKAgent
// File Name: KBCurator.ReviewIssue.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class ReviewIssue
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object ReviewItemId { get; set; }

        [StringLength(200)] public object Code { get; set; }

        [StringLength(50)] public object Severity { get; set; }

        [StringLength(1000)] public object RelatedMemberUid { get; set; }

        public object Details { get; set; }
    }
}