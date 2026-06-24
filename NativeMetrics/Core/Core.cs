using System;
using System.Runtime.InteropServices;

using NativeMetrics.Core.Models;

namespace NativeMetrics.Core;

public static class NativeMetricsCore
{
    private const string NATIVE_METRICS_DLL = "NativeMetrics_core.dll";

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong getTotalMemory();

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong getAvailableMemory();

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong getApproxPercentInUse();

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern double getCpuUsage();

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getProcessList([Out] ProcessInfo[] buffer, int bufferSize, out int processesWritten);

    [DllImport(NATIVE_METRICS_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool getNetworkAdapterInfo([Out] NetworkAdapterInfo[] buffer, int bufferSize, out int adaptersWritten);
}
