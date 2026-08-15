using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

class PerformanceManager
{
    public PerformanceViewModel PerformanceStats = new();
    
    public async Task RefreshAsync()
    {
        ulong totalMemory = NativeMetricsService.getTotalMemory();
        ulong availableMemory = NativeMetricsService.getAvailableMemory();
        ulong approxMemPercent = NativeMetricsService.getApproxPercentInUse();
        double cpuUsage = NativeMetricsService.getCpuUsage();

        Synchronize(totalMemory, availableMemory, approxMemPercent, cpuUsage);
    }
    
    private void Synchronize(ulong totalMemory, ulong availableMemory, ulong approxMemPercent, double cpuUsage)
    {
        PerformanceStats.TotalMemory = totalMemory;
        PerformanceStats.AvailableMemory = availableMemory;
        PerformanceStats.ApproxMemPercent = approxMemPercent;
        PerformanceStats.CpuUsage = cpuUsage;
    }
}
