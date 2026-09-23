using NativeMetrics.Services.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class PerformanceContext
{
    public CpuManager CpuManager { get; }
    public CpuUpdateService CpuUpdateService { get; }
    public MemoryManager MemoryManager { get; }
    public MemoryUpdateService MemoryUpdateService { get; }
    
    public PerformanceContext()
    {
        CpuManager = new CpuManager();
        CpuUpdateService = new CpuUpdateService(CpuManager);

        MemoryManager = new MemoryManager();
        MemoryUpdateService = new MemoryUpdateService(MemoryManager);
    }
}
