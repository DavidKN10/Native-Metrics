using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class PerformanceUpdateService
{
    private readonly DispatcherTimer _updateTimer = new();
    private readonly PerformanceManager _performanceManager;

    public PerformanceUpdateService(PerformanceManager performanceManager)
    {
        _performanceManager = performanceManager;
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
        RefreshManager();
    }

    public void StopTimer()
    {
        _updateTimer.Stop();
        _updateTimer.Tick -= OnTimerTick;
    }

    private async void RefreshManager()
    {
        await _performanceManager.RefreshAsync();
    }
}
