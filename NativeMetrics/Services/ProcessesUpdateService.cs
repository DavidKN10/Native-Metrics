using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

class ProcessesUpdateService
{
    private readonly DispatcherTimer _updateTimer = new();
    private readonly ProcessManager _processManager;

    // performance and network managers will be placed here

    public ProcessesUpdateService(ProcessManager processManager)
    {
        _processManager = processManager;
        InitializeTimer();
    }
    
    private void InitializeTimer()
    {
        _updateTimer.Interval = TimeSpan.FromSeconds(1);
        _updateTimer.Tick += OnTimerTick;
        _updateTimer.Start();
    }

    private void OnTimerTick(object? sender, object e)
    {
        RefreshManagers();
    }

    public void StopTimer()
    {
        _updateTimer.Stop();
        _updateTimer.Tick -= OnTimerTick;
    }

    private async void RefreshManagers()
    {
        await _processManager.RefreshAsync();
    }
}
