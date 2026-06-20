using System;

using NativeMetrics.Core;

namespace NativeMetrics;

public class Program
{
    static void Main(string[] args)
    {
        ulong totalMemory = NativeMetricsCore.getTotalMemory();
        ulong availableMemory = NativeMetricsCore.getAvailableMemory();
        ulong percentUse = NativeMetricsCore.getApproxPercentInUse();
        Console.WriteLine($"Total Memory: {totalMemory} GB");
        Console.WriteLine($"Available Memory: {availableMemory} GB");
        Console.WriteLine($"Percent in Use: {percentUse}%");
    }
}
