// Project Name: SKAgent
// File Name: KBCurator.SemanticIdentity.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class SemanticIdentity
{
    public class Metadata
    {
        [StringLength(1000)] [Required] public object Uid { get; set; }

        [Key] [Required] public object UidHash { get; set; }

        [StringLength(50)] [Required] public object Kind { get; set; }

        [Required] public object CreatedUtc { get; set; }

        public object Notes { get; set; }
    }
}