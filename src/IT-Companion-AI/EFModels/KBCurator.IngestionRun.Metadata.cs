// Project Name: SKAgent
// File Name: KBCurator.IngestionRun.Metadata.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


[MetadataType(typeof(Metadata))]
public partial class IngestionRun
{
    public class Metadata
    {
        [Key] [Required] public object Id { get; set; }

        [Required] public object TimestampUtc { get; set; }

        [StringLength(200)] [Required] public object SchemaVersion { get; set; }

        public object Notes { get; set; }

        public object ApiFeatures { get; set; }

        public object ApiMembers { get; set; }

        public object ApiParameters { get; set; }

        public object ApiTypes { get; set; }

        public object DocPages { get; set; }

        public object DocSections { get; set; }
    }
}