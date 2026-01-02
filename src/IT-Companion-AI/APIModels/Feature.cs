using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class Feature
{
    public Guid Id { get; set; }

    public Guid TruthRunId { get; set; }

    public string FeatureUid { get; set; } = null!;

    public string? Name { get; set; }

    public string? Language { get; set; }

    public string? Description { get; set; }

    public string? Tags { get; set; }

    public string? IntroducedInVersion { get; set; }

    public string? LastSeenVersion { get; set; }

    public virtual ICollection<FeatureDocLink> FeatureDocLinks { get; set; } = new List<FeatureDocLink>();

    public virtual ICollection<FeatureMemberLink> FeatureMemberLinks { get; set; } = new List<FeatureMemberLink>();

    public virtual ICollection<FeatureTypeLink> FeatureTypeLinks { get; set; } = new List<FeatureTypeLink>();

    public virtual TruthRun TruthRun { get; set; } = null!;
}
