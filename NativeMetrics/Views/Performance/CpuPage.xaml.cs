using LiveChartsCore.Measure;
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
    public sealed partial class CpuPage : Page
    {
        private CpuManager _manager;
        private CpuUpdateService _updateService;

        public CpuPage()
        {
            InitializeComponent();

            // modify chart 
            CpuUsageChart.XAxes = new[]
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

            CpuUsageChart.YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 100,
                    Labeler = value =>
                    {
                        if (value == 100) {
                            return "100%";
                        }
                        return "";
                    },
                    CustomSeparators = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                    ShowSeparatorLines = true,
                    SeparatorsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint
                    {
                        Color = SkiaSharp.SKColors.DarkGray,
                        StrokeThickness = 1
                    }
                }
            };

            this.Loaded += CpuPage_Loaded;
            this.Unloaded += CpuPage_Unloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var context = (PerformanceContext)e.Parameter;

            _manager = context.CpuManager;
            _updateService = context.CpuUpdateService;
        }

        private async void CpuPage_Loaded(object sender, RoutedEventArgs e)
        {
            _updateService?.StartTimer();
            await _manager.RefreshAsync();
        }

        private void CpuPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _updateService?.StopTimer();
        }
    }
}
