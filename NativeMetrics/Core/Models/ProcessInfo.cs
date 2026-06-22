using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeMetrics.Core.Models;

[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
public struct  ProcessInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string processName;

    public uint processId;
    public uint threadsCount;
    public uint parentProcessId;
    public int priorityClassBase;
    public uint priorityClass;

    public double memoryUsage;
    public double commitSize;
    public double privateMemory;
}
