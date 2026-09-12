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
    double pageSizeBytes;
    double totalMemoryBytes;
    double availableMemoryBytes;
    double memoryUsePercent;
    double commitCurrentBytes;
    double commitLimitBytes;
    double commitPeakBytes;
    double pagedPoolBytes;
    double nonPagedPoolBytes;
}
