using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

using NativeMetrics.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(titleBar);
            
            // show an initial page
            ContentFrame.Navigate(typeof(ProcessesPage));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                switch(item.Tag?.ToString())
                {
                    case "ProcessesPage":
                        ContentFrame.Navigate(typeof(ProcessesPage));
                        break;
                    case "PerformancePage":
                        ContentFrame.Navigate(typeof(PerformancePage));
                        break;
                    case "NetworkPage":
                        ContentFrame.Navigate(typeof(NetworkPage));
                        break;
                    case "SettingsPage":
                        ContentFrame.Navigate(typeof(SettingsPage));
                        break;
                }
            }

        }
    }
}
