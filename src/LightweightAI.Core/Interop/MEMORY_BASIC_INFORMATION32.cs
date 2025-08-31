// Project Name: LightweightAI.Core
// File Name: MEMORY_BASIC_INFORMATION32.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Runtime.InteropServices;



namespace LightweightAI.Core.Interop;


[StructLayout(LayoutKind.Sequential)]
internal struct MEMORY_BASIC_INFORMATION32
{
    public uint BaseAddress;
    public uint AllocationBase;
    public uint AllocationProtect;
    public uint RegionSize;
    public uint State;
    public uint Protect;
    public uint Type;
}