// Project Name: LightweightAI.Core
// File Name: QaQuestion.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


public sealed record QaQuestion(
    string EventId,
    DateTimeOffset Timestamp,
    string SourceId,
    string Text
);