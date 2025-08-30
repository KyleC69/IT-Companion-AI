// Project Name: LightweightAI.Core
// File Name: ILoggerSeverity.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


public interface ILoggerSeverity
{
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}