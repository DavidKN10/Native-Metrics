using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;

namespace NativeMetrics.Services;

public class MemoryManager
{
    public MemoryViewModel MemoryStats = new();

    public async Task RefreshAsync()
    {
        MemoryInfo memoryInfo = new();
        int bufferSize = Marshal.SizeOf(memoryInfo);

        bool result = NativeMetricsService.getMemoryInfo(ref memoryInfo, bufferSize);
        if (result)
        {
            Synchronize(memoryInfo);
        } 
    }

    private void Synchronize(MemoryInfo memory)
    {
        MemoryStats.Update(memory);
    }
}
