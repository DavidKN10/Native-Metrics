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
    public ulong pageSizeBytes;
    public ulong totalMemoryBytes;
    public ulong availableMemoryBytes;
    public ulong memoryUsePercent;
    public ulong commitCurrentBytes;
    public ulong commitLimitBytes;
    public ulong commitPeakBytes;
    public ulong pagedPoolBytes;
    public ulong nonPagedPoolBytes;
}
