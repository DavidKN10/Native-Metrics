using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NetworkAdapterInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string alias;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string description;

    public ulong luid;
    public ulong type;

    [MarshalAs(UnmanagedType.I1)]
    public bool isConnected;

    [MarshalAs(UnmanagedType.I1)]
    public bool isOperational;

    public ulong receivedLinkSpeedBits;
    public ulong transmitLinkSpeedBits;
    public ulong receivedBytes;
    public ulong sentBytes;

    public double downloadBytesPerSec;
    public double uploadBytesPerSec;
}
