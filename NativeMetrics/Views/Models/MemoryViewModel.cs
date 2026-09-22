using Microsoft.UI.Xaml.Media.Animation;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class MemoryViewModel : INotifyPropertyChanged
{
    private ulong _pageSizeBytes;
    private ulong _totalMemoryBytes;
    private ulong _availableMemoryBytes;
    private ulong _memoryUsePercent;
    private ulong _commitCurrentBytes;
    private ulong _commitLimitBytes;
    private ulong _commitPeakBytes;
    private ulong _pagedPoolBytes;
    private ulong _nonPagedPoolBytes;

    public ulong PageSizeBytes 
    { 
        get { return _pageSizeBytes; } 
        set { _pageSizeBytes = value; OnPropertyChanged(); }
    }
   
    public ulong TotalMemoryBytes 
    {  
        get { return _totalMemoryBytes; }
        set { _totalMemoryBytes = value; OnPropertyChanged(); }
    }
   
    public ulong CommitLimitBytes 
    { 
        get { return _commitLimitBytes; }
        set { _commitLimitBytes = value; OnPropertyChanged(); }
    }

    public ulong AvailableMemoryBytes
    {
        get { return _availableMemoryBytes; }
        set { _availableMemoryBytes = value; OnPropertyChanged(); }
    }

    public ulong MemoryUsePercent
    {
        get { return _memoryUsePercent; }
        set { _memoryUsePercent = value; OnPropertyChanged(); }
    }

    public ulong CommitCurrentBytes
    {
        get { return _commitCurrentBytes; }
        set { _commitCurrentBytes = value; OnPropertyChanged(); }
    }

    public ulong CommitPeakBytes
    {
        get { return _commitPeakBytes; }
        set { _commitPeakBytes = value; OnPropertyChanged(); }
    }

    public ulong PagedPoolBytes
    {
        get { return _pagedPoolBytes; }
        set { _pagedPoolBytes = value; OnPropertyChanged(); }
    }

    public ulong NonPagedPoolBytes
    {
        get { return _nonPagedPoolBytes; }
        set { _nonPagedPoolBytes = value; OnPropertyChanged(); }
    }

    public MemoryViewModel()
    { 
        _pageSizeBytes = 0;
        TotalMemoryBytes = 0;
        CommitLimitBytes = 0;
        _availableMemoryBytes = 0;
        _memoryUsePercent = 0;
        _commitCurrentBytes = 0;
        _commitPeakBytes = 0;
        _pagedPoolBytes = 0;
        _nonPagedPoolBytes = 0;
    }

    public void Update(MemoryInfo memory)
    {
        PageSizeBytes = memory.pageSizeBytes;
        TotalMemoryBytes = memory.totalMemoryBytes;
        CommitLimitBytes = memory.commitLimitBytes;
        AvailableMemoryBytes = memory.availableMemoryBytes;
        MemoryUsePercent = memory.memoryUsePercent;
        CommitCurrentBytes = memory.commitCurrentBytes;
        CommitPeakBytes = memory.commitPeakBytes;
        PagedPoolBytes = memory.pagedPoolBytes;
        NonPagedPoolBytes = memory.nonPagedPoolBytes;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
