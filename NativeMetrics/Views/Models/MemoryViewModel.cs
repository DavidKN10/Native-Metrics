using Microsoft.UI.Xaml.Media.Animation;
using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private FormattedValue _pageSizeFormatted;
    public FormattedValue PageSizeFormatted
    {
        get { return _pageSizeFormatted; }
        set { _pageSizeFormatted = value; OnPropertyChanged(); }
    }    

    private FormattedValue _totalMemoryFormatted;
    public FormattedValue TotalMemoryFormatted
    {
        get { return _totalMemoryFormatted; }
        set { _totalMemoryFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _availableMemoryFormatted;
    public FormattedValue AvailableMemoryFormatted
    {
        get { return _availableMemoryFormatted; }
        set { _availableMemoryFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _memoryUsePercentFormatted;
    public FormattedValue MemoryUsePercentFormatted
    {
        get { return _memoryUsePercentFormatted; }
        set { _memoryUsePercentFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _commitCurrentFormatted;
    public FormattedValue CommitCurrentFormatted
    {
        get { return _commitCurrentFormatted; }
        set { _commitCurrentFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _commitLimitFormatted;
    public FormattedValue CommitLimitFormatted
    {
        get { return _commitLimitFormatted; }
        set { _commitLimitFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _commitPeakFormatted;
    public FormattedValue CommitPeakFormatted
    {
        get { return _commitPeakFormatted; }
        set { _commitPeakFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _pagedPoolFormatted;
    public FormattedValue PagedPoolFormatted
    {
        get { return _pagedPoolFormatted; }
        set { _pagedPoolFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _nonPagedPoolFormatted;
    public FormattedValue NonPagedPoolFormatted
    {
        get { return _nonPagedPoolFormatted; }
        set { _nonPagedPoolFormatted = value; OnPropertyChanged(); }
    }

    public double MemoryUseBytes
    {
        get
        {
            return Formatter.BytesToGB(TotalMemoryBytes - AvailableMemoryBytes);
        }
    }

    public HistoryBuffer<double> MemoryUseHistory;
    public ObservableCollection<double> MemoryUseObservable { get; } = new();

    public MemoryViewModel()
    { 
        _pageSizeBytes = 0;
        TotalMemoryBytes = 0;
        _commitLimitBytes = 0;
        _availableMemoryBytes = 0;
        _memoryUsePercent = 0;
        _commitCurrentBytes = 0;
        _commitPeakBytes = 0;
        _pagedPoolBytes = 0;
        _nonPagedPoolBytes = 0;

        _pageSizeFormatted = new();
        _totalMemoryFormatted = new();
        _commitLimitFormatted = new();
        _availableMemoryFormatted = new();
        _memoryUsePercent = new();
        _commitCurrentFormatted = new();
        _commitPeakFormatted = new();
        _pagedPoolFormatted = new();
        _nonPagedPoolFormatted = new();

        MemoryUseHistory = new();
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

        PageSizeFormatted = Formatter.ByteMetricString(PageSizeBytes);
        TotalMemoryFormatted = Formatter.ByteMetricString(TotalMemoryBytes);
        CommitLimitFormatted = Formatter.ByteMetricString(CommitLimitBytes);
        AvailableMemoryFormatted = Formatter.ByteMetricString(AvailableMemoryBytes);
        MemoryUsePercentFormatted = Formatter.PercentString(MemoryUsePercent);
        CommitCurrentFormatted = Formatter.ByteMetricString(CommitCurrentBytes);
        CommitPeakFormatted = Formatter.ByteMetricString(CommitPeakBytes);
        PagedPoolFormatted = Formatter.ByteMetricString(PagedPoolBytes);
        NonPagedPoolFormatted = Formatter.ByteMetricString(NonPagedPoolBytes);
       
        MemoryUseHistory.Add(MemoryUseBytes);
        if (MemoryUseObservable.Count >= 61)
        {
            MemoryUseObservable.RemoveAt(0);
        }
        MemoryUseObservable.Add(MemoryUseBytes);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
