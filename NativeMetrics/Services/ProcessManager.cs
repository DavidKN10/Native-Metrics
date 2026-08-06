using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

class ProcessManager
{
    public ObservableCollection<ProcessesViewModel> Processes { get; } = new();

    private readonly Dictionary<uint, ProcessesViewModel> processLookup = new Dictionary<uint, ProcessesViewModel>();

    public Task RefreshAsync()
    {
        ProcessInfo[] newProcesses = RetrieveProcessSnapshot();

        Synchronize(newProcesses);

        return Task.CompletedTask;
    }

    private ProcessInfo[] RetrieveProcessSnapshot()
    {
        ProcessInfo[] processList = new ProcessInfo[256];

        NativeMetricsService.getProcessList(processList, processList.Length, out int processesWritten);

        return processList;
    }
    
    private void Synchronize(ProcessInfo[] snapshot)
    {
        HashSet<uint> snapshotProcessIds = BuildSnapshotPidSet(snapshot);
 
        foreach(ProcessInfo process in snapshot)
        {
            // update process in Dictionary and ObservableCollection
            if (processLookup.ContainsKey(process.processId))
            {
                processLookup[process.processId].ThreadsCount = process.threadsCount;
                processLookup[process.processId].MemoryUsage = process.memoryUsage;
                processLookup[process.processId].PrivateMemory = process.privateMemory;

                ProcessesViewModel? processToUpdate = Processes.FirstOrDefault(x => x.ProcessId == process.processId);
                if (processToUpdate != null)
                {
                    processToUpdate.Update(process);
                }
                
            }
            // add new process to Dictionary and ObservableCollection
            else
            {
                processLookup.Add(process.processId, new ProcessesViewModel(process));

                Processes.Add(new ProcessesViewModel(process));
            }
        }

        // remove exited processes in 
        foreach (var (processId, process) in processLookup)
        {
            if (!snapshotProcessIds.Contains(processId))
            {
                processLookup.Remove(processId);

                ProcessesViewModel? processToRemove = Processes.FirstOrDefault(x => x.ProcessId == processId);
                if (processToRemove != null)
                {
                    Processes.Remove(processToRemove);
                }
            }
        }         
        
    }

    private HashSet<uint> BuildSnapshotPidSet(ProcessInfo[] snapshot)
    {
        HashSet<uint> processIds = new HashSet<uint>();
        foreach (ProcessInfo process in snapshot)
        {
            processIds.Add(process.processId);
        }
        return processIds;
    }
}
