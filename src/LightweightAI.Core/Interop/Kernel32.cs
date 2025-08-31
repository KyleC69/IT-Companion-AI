// Project Name: LightweightAI.Core
// File Name: Kernel32.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Runtime.InteropServices;
using System.Text;



namespace LightweightAI.Core.Interop;


internal static class Kernel32
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(uint desiredAccess, bool inheritHandle, uint processId);





    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);





    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int VirtualQueryEx(
        IntPtr hProcess,
        UIntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION64 lpBuffer,
        UIntPtr dwLength);





    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int VirtualQueryEx(
        IntPtr hProcess,
        UIntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION32 lpBuffer,
        UIntPtr dwLength);





    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint QueryDosDevice(
        string lpDeviceName,
        StringBuilder lpTargetPath,
        uint ucchMax);
}