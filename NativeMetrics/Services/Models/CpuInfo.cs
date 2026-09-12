using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct CpuInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string processorName;

    public uint logicalProcessors;
    public uint cores;
    public uint baseSpeed;
    public uint handles;
    public uint processCount;
    public uint threadCount;

    public double cpuUsage;
}

