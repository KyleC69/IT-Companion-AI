// Project Name: LightweightAI.Core
// File Name: QaRefinery.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.DataRefineries;


public sealed class QaRefinery
{
    public QaQuestion Process(QaQuestion raw)
    {
        if (raw is null)
            throw new ArgumentNullException(nameof(raw));

        EventIdValidator.Enforce(raw.EventId);

        // Optional: sanitize text, trim whitespace, apply casing rules, etc.
        return raw;
    }
}