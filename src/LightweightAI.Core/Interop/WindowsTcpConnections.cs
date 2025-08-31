// Project Name: ${File.ProjectName}
// File Name: ${File.FileName}
// Author: ${User.FullName}
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace LightweightAI.Core.Interop;

internal static class WindowsTcpConnections
{
    // Table class values (from IPHlpApi.h)
    private enum TCP_TABLE_CLASS : int
    {
        TCP_TABLE_BASIC_LISTENER,
        TCP_TABLE_BASIC_CONNECTIONS,
        TCP_TABLE_BASIC_ALL,
        TCP_TABLE_OWNER_PID_LISTENER,
        TCP_TABLE_OWNER_PID_CONNECTIONS,
        TCP_TABLE_OWNER_PID_ALL,
        TCP_TABLE_OWNER_MODULE_LISTENER,
        TCP_TABLE_OWNER_MODULE_CONNECTIONS,
        TCP_TABLE_OWNER_MODULE_ALL
    }

    // MIB_TCPROW_OWNER_PID (IPv4)
    [StructLayout(LayoutKind.Sequential)]
    private struct MIB_TCPROW_OWNER_PID
    {
        public uint State;
        public uint LocalAddr;
        public uint LocalPort;
        public uint RemoteAddr;
        public uint RemotePort;
        public uint OwningPid;
    }

    // MIB_TCPTABLE_OWNER_PID
    [StructLayout(LayoutKind.Sequential)]
    private struct MIB_TCPTABLE_OWNER_PID
    {
        public uint NumEntries;
        // Followed by NumEntries MIB_TCPROW_OWNER_PID structs
    }

    [DllImport("iphlpapi.dll", SetLastError = true)]
    private static extern uint GetExtendedTcpTable(
        IntPtr pTcpTable,
        ref int dwOutBufLen,
        bool sort,
        int ipVersion,
        TCP_TABLE_CLASS tableClass,
        uint reserved = 0);

    // State codes (subset)
    private static readonly string[] _stateNames =
    {
        "UNKNOWN", "CLOSED", "LISTEN", "SYN-SENT", "SYN-RECEIVED",
        "ESTABLISHED", "FIN-WAIT-1", "FIN-WAIT-2", "CLOSE-WAIT",
        "CLOSING", "LAST-ACK", "TIME-WAIT", "DELETE-TCB"
    };

    private static string StateToString(uint state)
    {
        if (state < _stateNames.Length) return _stateNames[state];
        return $"STATE-{state}";
    }

    private static int ParsePort(uint portNetOrder)
    {
        // dwLocalPort contains the port in the LOW 16 bits, network byte order
        return (ushort)IPAddress.NetworkToHostOrder((short)portNetOrder);
    }

    public sealed record TcpRow(
        string LocalAddress,
        int LocalPort,
        string RemoteAddress,
        int RemotePort,
        string State,
        int ProcessId);

    public static IEnumerable<TcpRow> GetAllTcpWithPidIPv4()
    {
        int size = 0;
        uint err = GetExtendedTcpTable(IntPtr.Zero, ref size, true, 2 /*AF_INET*/, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
        if (err != 0 && err != 122 /*ERROR_INSUFFICIENT_BUFFER*/)
            yield break;

        IntPtr buffer = IntPtr.Zero;
        try
        {
            buffer = Marshal.AllocHGlobal(size);
            err = GetExtendedTcpTable(buffer, ref size, true, 2, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
            if (err != 0)
                yield break;

            // First uint = number of entries
            uint numEntries = (uint)Marshal.ReadInt32(buffer);
            IntPtr rowPtr = buffer + sizeof(uint);
            int rowSize = Marshal.SizeOf<MIB_TCPROW_OWNER_PID>();

            for (int i = 0; i < numEntries; i++)
            {
                var row = Marshal.PtrToStructure<MIB_TCPROW_OWNER_PID>(rowPtr);

                string localAddr = new IPAddress(row.LocalAddr).ToString();
                string remoteAddr = new IPAddress(row.RemoteAddr).ToString();
                int localPort = ParsePort(row.LocalPort);
                int remotePort = ParsePort(row.RemotePort);
                string state = StateToString(row.State);
                int pid = unchecked((int)row.OwningPid);

                yield return new TcpRow(localAddr, localPort, remoteAddr, remotePort, state, pid);

                rowPtr += rowSize;
            }
        }
        finally
        {
            if (buffer != IntPtr.Zero)
                Marshal.FreeHGlobal(buffer);
        }
    }
}
