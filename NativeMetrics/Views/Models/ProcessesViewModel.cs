using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class ProcessesViewModel
{
    public string? ProcessName { get; }
    public uint ProcessId {  get; }
    public uint ThreadsCount { get; set;  }
    public uint ParentProcessId { get; }
    public int PriorityClassBase { get; }
    public uint PriorityClass { get; }
    public double MemoryUsage { get; set; }
    public double CommitSize { get; }
    public double PrivateMemory {  get; set; }

    public ProcessesViewModel(ProcessInfo process) 
    {
        ProcessName = process.processName;
        ProcessId = process.processId;
        ThreadsCount = process.threadsCount;
        ParentProcessId = process.parentProcessId;
        PriorityClassBase = process.priorityClassBase;
        PriorityClass = process.priorityClass;
        MemoryUsage = process.memoryUsage;
        CommitSize = process.commitSize;
        PrivateMemory = process.privateMemory;
    }

    public void Update(ProcessInfo process)
    {
        ThreadsCount = process.threadsCount;
        MemoryUsage = process.memoryUsage;
        PrivateMemory = process.privateMemory;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {

    }

}
