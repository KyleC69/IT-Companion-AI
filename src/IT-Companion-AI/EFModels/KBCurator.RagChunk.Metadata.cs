// Project Name: SKAgent
// File Name: KBCurator.RagChunk.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class RagChunk
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object RagRunId { get; set; }

        [StringLength(1000)] [Required] public object ChunkUid { get; set; }

        [StringLength(100)] public object Kind { get; set; }

        public object Text { get; set; }

        public object MetadataJson { get; set; }

        [StringLength(1536)] public object EmbeddingVector { get; set; }
    }
}