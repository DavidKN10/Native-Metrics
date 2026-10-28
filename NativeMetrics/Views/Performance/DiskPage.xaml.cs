using NativeMetrics.Views.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Web.WebView2.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics.Views.Performance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiskPage : Page
    {
        private DispatcherTimer _updateTimer;
        public DiskPage()
        {
            InitializeComponent();

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += UpdateTransferRate;
            _updateTimer.Start();

            // modify chart
            ActiveTimeChart.XAxes = new[]
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
                    CustomSeparators = [0, 5, 10 , 15, 20, 25, 30, 35, 40, 45, 50, 55, 60],
                    ShowSeparatorLines = true,
                    SeparatorsPaint  = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint
                    {
                        Color = SkiaSharp.SKColors.DarkGray,
                        StrokeThickness = 1
                    }
                }
            };

            ActiveTimeChart.YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 100,
                    Labeler = value =>
                    {
                        if (value == 100)
                        {
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

            TransferRateChart.XAxes = new[]
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
                    CustomSeparators = [0, 5, 10 , 15, 20, 25, 30, 35, 40, 45, 50, 55, 60],
                    ShowSeparatorLines = true,
                    SeparatorsPaint  = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint
                    {
                        Color = SkiaSharp.SKColors.DarkGray,
                        StrokeThickness = 1
                    }
                }
            };
            this.Loaded += DiskPage_Loaded;
        }

        private void UpdateTransferRate(object sender, object e)
        {
            var viewModel = DataContext as DiskViewModel;
         
            var yAxis = TransferRateChart.YAxes.FirstOrDefault();
            if (yAxis is null)
            {
                return;
            }

            yAxis.MaxLimit = viewModel?.TransferRateMax;
            yAxis.Labeler = value =>
            {
                return $"{value:F1} {viewModel?.TransferRateHistory.Unit}";
            };
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter as DiskViewModel;
        }

        private async void DiskPage_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DiskViewModel;
            TransferRateChart.YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = viewModel?.TransferRateMax,
                    Labeler = value =>
                    {
                        return $"{value:F1} {viewModel?.TransferRateHistory.Unit}";
                    },
                    ShowSeparatorLines = true,
                    SeparatorsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint
                    {
                        Color = SkiaSharp.SKColors.DarkGray,
                        StrokeThickness = 1
                    }
                }
            };
        }
    }
}
