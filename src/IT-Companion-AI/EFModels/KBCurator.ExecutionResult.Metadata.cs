// Project Name: SKAgent
// File Name: KBCurator.ExecutionResult.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class ExecutionResult
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object ExecutionRunId { get; set; }

        [StringLength(1000)] [Required] public object SampleUid { get; set; }

        [StringLength(100)] public object Status { get; set; }

        public object BuildLog { get; set; }

        public object RunLog { get; set; }

        public object ExceptionJson { get; set; }

        public object DurationMs { get; set; }
    }
}