using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics.Views.Network
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NetworkAdapterPage : Page
    {
        private DispatcherTimer _updateTimer;
        public NetworkAdapterPage()
        {
            InitializeComponent();

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += UpdateThroughput;
            _updateTimer.Start();

            // modify chart
            ThroughputChart.XAxes = new[]
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

            this.Loaded += NetworkAdapterPage_Loaded;
        }

        private void UpdateThroughput(object sender, object e)
        {
            var viewModel = DataContext as NetworkViewModel;

            var yAxis = ThroughputChart.YAxes.FirstOrDefault();
            if (yAxis is null)
            {
                return;
            }

            yAxis.MaxLimit = viewModel?.ThroughputMax;
            yAxis.Labeler = value =>
            {
                if (value == 0)
                {
                    return string.Empty;
                }
                return $"{value:F1} {viewModel?.ThroughputHistory.Unit}";
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter as NetworkViewModel;
        }

        private async void NetworkAdapterPage_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NetworkViewModel;
            ThroughputChart.YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = viewModel?.ThroughputMax,
                    Labeler = value =>
                    {
                        if (value == 0)
                        {
                            return string.Empty;
                        }
                        return $"{value:F1} {viewModel?.ThroughputHistory.Unit}";
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
