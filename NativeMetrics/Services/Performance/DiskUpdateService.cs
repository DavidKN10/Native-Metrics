using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Performance;

public class DiskUpdateService
{
    private readonly DispatcherTimer _updateTimer = new();
    private readonly DiskManager _diskManager;

    public DiskUpdateService(DiskManager diskManager)
    {
        _diskManager = diskManager;
        InitializeTimer();
    }
    
    private void InitializeTimer()
    {
        _updateTimer.Interval = TimeSpan.FromSeconds(1);
        _updateTimer.Tick += OnTimerTick;
        _updateTimer.Start();
    }

    public void StartTimer()
    {
        _updateTimer.Tick += OnTimerTick;
        _updateTimer.Start();
    }

    public void StopTimer()
    {
        _updateTimer.Stop();
        _updateTimer.Tick -= OnTimerTick;
    }

    private void OnTimerTick(object? sender, object e)
    {
        RefreshManager();
    }

    private async void RefreshManager()
    {
        await _diskManager.RefreshAsync();
    }
}
