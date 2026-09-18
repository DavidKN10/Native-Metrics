using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class CpuManager
{
    public CpuViewModel CpuStats = new();

    public async Task RefreshAsync()
    {
        CpuInfo cpuInfo = new();
        int bufferSize = Marshal.SizeOf<CpuInfo>();

        bool result = NativeMetricsService.getCpuInfo(ref cpuInfo, bufferSize);
        
        if (result)
        {
            CpuStats.SetStats(cpuInfo);
            Synchronize(cpuInfo); 
        }
    }

    private void Synchronize(CpuInfo cpu)
    {
        CpuStats.Update(cpu);
    }
}
