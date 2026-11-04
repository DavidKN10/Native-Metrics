using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct SystemInfo 
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string deviceName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string systemType;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string windowsVersion;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string windowsEdition;
    
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string hostname;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string firmware;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string processor;

    public ulong buildNumber;
    public ulong installedRam;
    public ulong storageCapacity;
    public ulong usedStorage;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct GpuSummary
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string name;
}
