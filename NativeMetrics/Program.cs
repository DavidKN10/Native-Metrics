using System;

using NativeMetrics.Core;

namespace NativeMetrics;

public class Program
{
    public static void getBasicStats()
    {
        ulong totalMemory = NativeMetricsCore.getTotalMemory();
        ulong availableMemory = NativeMetricsCore.getAvailableMemory();
        ulong percentUse = NativeMetricsCore.getApproxPercentInUse();
        double cpuUsage = NativeMetricsCore.getCpuUsage();

        Console.WriteLine($"Total RAM: {totalMemory} GB");
        Console.WriteLine($"Available RAM: {availableMemory} GB ({percentUse}% in use)");
        Console.WriteLine($"CPU Usage: {cpuUsage.ToString("F2")}%");
    }

    static void Main(string[] args)
    {
        getBasicStats();
    }
}
