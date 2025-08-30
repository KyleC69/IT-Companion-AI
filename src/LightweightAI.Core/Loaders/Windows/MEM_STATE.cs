// Project Name: LightweightAI.Core
// File Name: MEM_STATE.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


internal enum MEM_STATE : uint
{
    MEM_COMMIT = 0x1000,
    MEM_RESERVE = 0x2000,
    MEM_FREE = 0x10000
}