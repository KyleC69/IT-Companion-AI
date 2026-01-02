using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class Sample
{
    public Guid Id { get; set; }

    public Guid SampleRunId { get; set; }

    public string SampleUid { get; set; } = null!;

    public string? FeatureUid { get; set; }

    public string? Language { get; set; }

    public string? Code { get; set; }

    public string? EntryPoint { get; set; }

    public string? TargetFramework { get; set; }

    public string? PackageReferences { get; set; }

    public string? DerivedFromCodeUid { get; set; }

    public string? Tags { get; set; }

    public virtual ICollection<SampleApiMemberLink> SampleApiMemberLinks { get; set; } = new List<SampleApiMemberLink>();

    public virtual SampleRun SampleRun { get; set; } = null!;
}
