using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using NativeMetrics.Services.Models;

namespace NativeMetrics.Services;

public static class NativeMetricsService
{
    private const string NATIVE_METRICS_DLL = "NativeMetrics_core.dll";

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getCpuInfo([Out] CpuInfo buffer, int bufferSize);

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getDiskInfo([Out] DiskInfo buffer, int bufferSize, out int disksWritten);

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getGpuInfo([Out] GpuInfo buffer, int bufferSize, out int adaptersWritten);

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getMemoryInfo([Out] MemoryInfo buffer, int bufferSize);
    
    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getProcessList([Out] ProcessInfo[] buffer, int bufferSize, out int processesWritten);  
   
    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getNetworkAdapterInfo([Out] NetworkAdapterInfo[] buffer, int bufferSize, out int adaptersWritten);
}
