// Project Name: SKAgent
// File Name: KBCurator.FeatureDocLink.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class FeatureDocLink
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object FeatureId { get; set; }

        [StringLength(1000)] [Required] public object DocUid { get; set; }

        [StringLength(1000)] public object SectionUid { get; set; }
    }
}