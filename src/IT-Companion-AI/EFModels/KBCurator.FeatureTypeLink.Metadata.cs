// Project Name: SKAgent
// File Name: KBCurator.FeatureTypeLink.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class FeatureTypeLink
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object FeatureId { get; set; }

        [StringLength(1000)] [Required] public object TypeUid { get; set; }

        [StringLength(50)] public object Role { get; set; }
    }
}