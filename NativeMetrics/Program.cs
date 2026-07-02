using System;

using NativeMetrics.Core;
using NativeMetrics.Core.Models;

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

    public static void getProcessesStats()
    {
        var processes = new ProcessInfo[200];
        Console.WriteLine(
                 $"Process Info:\n" +
                 $"{"PID",-8} " +
                 $"{"Process Name",-30} " +
                 $"{"Memory (MB)",-12} " +
                 $"{"Commit (MB)",-12} " +
                 $"{"Private (MB)",-12} " +
                 $"{"Threads",-8}"
             );
        Console.WriteLine(new string('-', 90));

        if (NativeMetricsCore.getProcessList(processes, processes.Length, out int processesWritten))
        {
            for (int i = 0; i < processesWritten; i++)
            {
                Console.WriteLine(
                    $"{processes[i].processId,-8}" +
                    $"{processes[i].processName,-30}" +
                    $"{processes[i].memoryUsage,12:F1}" +
                    $"{processes[i].commitSize,12:F1}" +
                    $"{processes[i].privateMemory,12:F1}" +
                    $"{processes[i].threadsCount,8}"
                );
            }
        }
    }

    public static void getNetworkStats()
    {
        var networkInfo = new NetworkAdapterInfo[200];

        if (NativeMetricsCore.getNetworkAdapterInfo(networkInfo, networkInfo.Length, out int adaptersWritten))
        {
            for (int i = 0; i < adaptersWritten; i++)
            {
                Console.WriteLine(
                    $"Alias: {networkInfo[i].alias}\n" +
                    $"Locally Unique ID: {networkInfo[i].luid}\n" +
                    $"Type: {networkInfo[i].type}\n"+
                    $"Description: {networkInfo[i].description}\n" +
                    $"Connected: {networkInfo[i].isConnected}\n" +
                    $"Operational: {networkInfo[i].isOperational}\n" +
                    $"Download Speed: {networkInfo[i].downloadBytesPerSec,0:F2} B/s\n" +
                    $"Upload Speed: {networkInfo[i].uploadBytesPerSec,0:F2} B/s\n\n"
                );
            }
        }
    }

    static void Main(string[] args)
    {
        while (true)
        {
            Thread.Sleep(1000);
            getNetworkStats();
        }
    }
}
