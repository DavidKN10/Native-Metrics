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
    private double _pageSizeBytes;
    private double _totalMemoryBytes;
    private double _availableMemoryBytes;
    private double _memoryUsePercent;
    private double _commitCurrentBytes;
    private double _commitLimitBytes;
    private double _commitPeakBytes;
    private double _pagedPoolBytes;
    private double _nonPagedPoolBytes;

    public double PageSizeBytes { get { return _pageSizeBytes; } }
    public double TotalMemoryBytes {  get { return _totalMemoryBytes; } }
    public double CommitLimitBytes { get { return _commitLimitBytes; } }

    public double AvailableMemoryBytes
    {
        get { return _availableMemoryBytes; }
        set { _availableMemoryBytes = value; OnPropertyChanged(); }
    }

    public double MemoryUsePercent
    {
        get { return _memoryUsePercent; }
        set { _memoryUsePercent = value; OnPropertyChanged(); }
    }

    public double CommitCurrentBytes
    {
        get { return _commitCurrentBytes; }
        set { _commitCurrentBytes = value; OnPropertyChanged(); }
    }

    public double CommitPeakBytes
    {
        get { return _commitPeakBytes; }
        set { _commitPeakBytes = value; OnPropertyChanged(); }
    }

    public double PagedPoolBytes
    {
        get { return _pagedPoolBytes; }
        set { _pagedPoolBytes = value; OnPropertyChanged(); }
    }

    public double NonPagedPoolBytes
    {
        get { return _nonPagedPoolBytes; }
        set { _nonPagedPoolBytes = value; OnPropertyChanged(); }
    }

    public MemoryViewModel()
    {
        _availableMemoryBytes = 0.0;
        _memoryUsePercent = 0.0;
        _commitCurrentBytes = 0.0;
        _commitPeakBytes = 0.0;
        _pagedPoolBytes = 0.0;
        _nonPagedPoolBytes = 0.0;
    }

    public void Update(MemoryInfo memory)
    {
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
