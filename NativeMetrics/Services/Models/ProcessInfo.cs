using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct ProcessInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string processName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string processPath;

    public uint processId;
    public uint threadsCount;
    public uint parentProcessId;
    public int priorityClassBase;
    public uint priorityClass;

    public double memoryUsage;
    public double commitSize;
    public double privateMemory;
}
