using ABI.Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NativeMetrics.Services;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics.Views.Network
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NetworkPage : Page
    {
        private readonly NetworkManager _manager;
        private readonly NetworkUpdateService _updateService;
        public NetworkPage()
        {
            InitializeComponent();

            _manager = new();
            _updateService = new(_manager);
            _manager.Networks.CollectionChanged += Network_CollectionChanged;

            this.Loaded += NetworkPage_Loaded;
            this.Unloaded += NetworkPage_Unloaded;
        }

        private async void NetworkPage_Loaded(object sender, RoutedEventArgs e)
        {
            await _manager.RefreshAsync();
        }

        private async void NetworkPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _updateService.StopTimer();
        }

        private void NetworkNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                if (item.Tag is NetworkViewModel network)
                {
                    NetworkContentFrame.Navigate(typeof(NetworkAdapterPage), network);
                    return;
                }
            }
        }

        private void Network_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SynchornizeNetworkAdapterNavigationItems();
        }


        private void SynchornizeNetworkAdapterNavigationItems()
        {
            var networks = _manager.Networks;
            var networkItems = NetworksNavigationView.MenuItems
                .OfType<NavigationViewItem>()
                .Where(item => item.Tag is NetworkViewModel)
                .ToList();

            foreach(NavigationViewItem item in networkItems)
            {
                if (!networks.Contains((NetworkViewModel)item.Tag))
                {
                    NetworksNavigationView.MenuItems.Remove(item);
                }
            }

            foreach(NetworkViewModel network in networks)
            {
                if (networkItems.Any(item => ReferenceEquals(item.Tag, network)))
                {
                    continue;
                }

                string label = $"{network.Alias}";
                NetworksNavigationView.MenuItems.Add(new NavigationViewItem
                {
                    Content = string.IsNullOrWhiteSpace(label) ? "Network" : label,
                    Tag = network
                });
            }
        }
    }
}
