using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NativeMetrics.Services;
using NativeMetrics.Services.Performance;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemoryPage : Page
    {
        private MemoryManager _manager;
        private MemoryUpdateService _updateService;
        private MemoryViewModel _viewModel;
        private double _memoryLimitGB = 0;

        public MemoryPage()
        {
            InitializeComponent();

            MemoryUseChart.XAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 60,
                    Labeler = value =>
                    {
                        if (value == 0)
                        {
                            return "0";
                        }
                        if (value == 60)
                        {
                            return "60 seconds";
                        }
                        return "";
                    },
                    CustomSeparators = [0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60],
                    ShowSeparatorLines = true,
                    SeparatorsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint
                    {
                        Color = SkiaSharp.SKColors.DarkGray,
                        StrokeThickness = 1
                    }
                }
            };
            
            this.Loaded += MemoryPage_Loaded;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var context = (PerformanceContext)e.Parameter;
            
            _manager = context.MemoryManager;
            _updateService = context.MemoryUpdateService;
            _viewModel = _manager.MemoryStats;
            _memoryLimitGB = Formatter.BytesToGB(_viewModel.TotalMemoryBytes);
        }

        private double[] CreateSeparators(double maxLimit, int numberOfSeparators)
        {
            return Enumerable.Range(0, numberOfSeparators + 1)
                             .Select(i => i * _memoryLimitGB / numberOfSeparators)
                             .ToArray();
        }

        private async void MemoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            var separatos = CreateSeparators(_memoryLimitGB, 5);

            MemoryUseChart.YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = _memoryLimitGB,
                    Labeler = value =>
                    {
                        if (value == _memoryLimitGB)
                        {
                            return $"{value:F1} GB";
                        }
                        return string.Empty;
                    },
                    CustomSeparators = separatos,
                    ShowSeparatorLines = true,
                    SeparatorsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint
                    {
                        Color = SkiaSharp.SKColors.DarkGray,
                        StrokeThickness = 1
                    }
                }
            };

            _updateService?.StartTimer();
            await _manager.RefreshAsync();
        }
    }
}
