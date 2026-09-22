using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;

namespace NativeMetrics.Services;

public class MemoryUpdateService
{
    private readonly DispatcherTimer _updateTimer = new();
    private readonly MemoryManager _memoryManger;

    public MemoryUpdateService(MemoryManager memoryManager)
    {
        _memoryManger = memoryManager;
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        _updateTimer.Interval = TimeSpan.FromSeconds(1);
        _updateTimer.Tick += OnTimerTick;
        _updateTimer.Start();
    }

    private void OnTimerTick(object? senfer, object e)
    {
        RefreshManager();
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

    private async void RefreshManager()
    {
        await _memoryManger.RefreshAsync();
    }
}
