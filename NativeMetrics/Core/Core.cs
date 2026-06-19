using System;
using System.Runtime.InteropServices;

namespace NativeMetrics.Core;

public static class NativeMetricsCore
{
    private const string dll = "NativeMetrics_core.dll";

    [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
    public static extern void test();
    
}
