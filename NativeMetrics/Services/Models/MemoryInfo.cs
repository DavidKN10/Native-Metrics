using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct MemoryInfo
{
    public double pageSizeBytes;
    public double totalMemoryBytes;
    public double availableMemoryBytes;
    public double memoryUsePercent;
    public double commitCurrentBytes;
    public double commitLimitBytes;
    public double commitPeakBytes;
    public double pagedPoolBytes;
    public double nonPagedPoolBytes;
}
