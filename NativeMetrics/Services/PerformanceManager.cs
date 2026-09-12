using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class PerformanceManager
{
    public PerformanceViewModel PerformanceStats = new();
    
    public async Task RefreshAsync()
    {
        ulong totalMemory = 1;
        ulong availableMemory = 2;
        ulong approxMemPercent = 3;
        double cpuUsage = 4;

        Synchronize(totalMemory, availableMemory, approxMemPercent, cpuUsage);
    }
    
    private void Synchronize(ulong totalMemory, ulong availableMemory, ulong approxMemPercent, double cpuUsage)
    {
        PerformanceStats.Update(totalMemory, availableMemory, approxMemPercent, cpuUsage); 
    }
}
