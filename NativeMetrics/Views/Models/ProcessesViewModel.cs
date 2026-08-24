using Microsoft.UI.Xaml.Media.Imaging;
using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class ProcessesViewModel : INotifyPropertyChanged
{
    private string? _processName;
    private string? _processPath;
    private uint _processId;
    private uint _parentProcessId;
    private int _priorityClassBase;
    private uint _priorityClass;
    private double _commitSize;
    private double _memoryUsage;
    private uint _threadsCount;
    private double _privateMemory;
    private SoftwareBitmapSource? _iconSource;

    public string? ProcessName { get { return _processName; } }
    public string? ProcessPath { get { return _processPath; } }
    public uint ProcessId { get { return _processId; } }
    public uint ParentProcessId { get { return _parentProcessId; } }
    public int PriorityClassBase { get { return _priorityClassBase; } }
    public uint PriorityClass { get { return _priorityClass; } }
    public double CommitSize { get { return _commitSize; } }

    public double MemoryUsage { 
        get {  return _memoryUsage; } 
        set { _memoryUsage = value; OnPropertyChanged(); } 
    }

    public uint ThreadsCount {
        get { return _threadsCount; } 
        set { _threadsCount = value; OnPropertyChanged(); }
    }

    public double PrivateMemory { 
        get { return _privateMemory; } 
        set { _privateMemory = value; OnPropertyChanged(); }
    }

    public SoftwareBitmapSource? IconSource { 
        get { return _iconSource; } 
        set { _iconSource = value; OnPropertyChanged(); }
    }

    public ProcessesViewModel(ProcessInfo process) 
    {
        _processName = process.processName;
        _processPath = process.processPath;
        _processId = process.processId;
        _threadsCount = process.threadsCount;
        _parentProcessId = process.parentProcessId;
        _priorityClassBase = process.priorityClassBase;
        _priorityClass = process.priorityClass;
        _memoryUsage = process.memoryUsage;
        _commitSize = process.commitSize;
        _privateMemory = process.privateMemory;
    }

    public void Update(ProcessInfo process)
    {
        ThreadsCount = process.threadsCount;
        MemoryUsage = process.memoryUsage;
        PrivateMemory = process.privateMemory;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
