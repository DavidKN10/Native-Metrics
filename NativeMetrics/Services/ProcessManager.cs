using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

class ProcessManager
{
    public ObservableCollection<ProcessesViewModel> Processes { get; } = new();
}
