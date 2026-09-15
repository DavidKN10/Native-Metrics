using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct GpuInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string description;

    public ulong luid;
    public uint vendorId;

    public double dedicatedVideoMemoryBytes;
    public double localCurrentMemoryBytes;
    public double currentUsagePercent;

    public double sharedSystemMemoryBytes;
    public double nonLocalCurrentMemoryBytes;
}
