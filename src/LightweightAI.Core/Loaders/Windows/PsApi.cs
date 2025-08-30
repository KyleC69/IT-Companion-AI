// Project Name: LightweightAI.Core
// File Name: PsApi.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Runtime.InteropServices;
using System.Text;


namespace LightweightAI.Core.Loaders.Windows;


internal static class PsApi
{
    [DllImport("psapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint GetMappedFileNameW(
        IntPtr hProcess,
        UIntPtr lpv,
        StringBuilder lpFilename,
        uint nSize);
}