// Project Name: LightweightAI.Core
// File Name: MEMORY_BASIC_INFORMATION64.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Runtime.InteropServices;


namespace LightweightAI.Core.Loaders.Windows;


[StructLayout(LayoutKind.Sequential)]
internal struct MEMORY_BASIC_INFORMATION64
{
    public ulong BaseAddress;
    public ulong AllocationBase;
    public uint AllocationProtect;
    public uint __alignment1;
    public ulong RegionSize;
    public uint State;
    public uint Protect;
    public uint Type;
    public uint __alignment2;
}