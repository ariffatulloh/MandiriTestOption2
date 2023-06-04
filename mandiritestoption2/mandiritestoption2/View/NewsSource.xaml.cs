using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using News.Helpers;
using News.Models.Responses;
using News.ViewModels;
using Xamarin.Forms;

namespace News
{
    public partial class NewsSource : ContentPage
    {
        private string searchText;
        private readonly TimeSpan debounceDelay = TimeSpan.FromSeconds(5);
        private Timer debounceTimer;
        NewsSourceViewModel vm = null;

        public NewsSource()
        {
            InitializeComponent();
            LoadViewModel();
        }

        public void LoadViewModel()
        {
            if (vm == null)
            {
                vm = new NewsSourceViewModel();
                BindingContext = vm;
            }
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var source = ((ListView)sender).SelectedItem as SourceModel;
            if (source == null)
                return;

            Settings.Source = source.Id;
            Settings.SourceName = source.Name;

            await Navigation.PushAsync(new NewsPage(source), true);
        }
        
        //private void OnSearchButtonPressed(object sender, EventArgs e)
        //{
        //    string searchText = searchBar.Text;
        //    Debug.WriteLine(""+searchText);
        //    vm.FilterSources(searchText);
        //}
        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = e.NewTextValue;
            if (debounceTimer != null)
            {
                debounceTimer.Dispose();
                debounceTimer = null;
            }

            debounceTimer = new Timer(PerformSearch, null,3000, Timeout.Infinite);
        }
        private void PerformSearch(object state)
        {
            
            Debug.WriteLine("" + searchText);
            
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Perform search or filtering logic here based on the searchText
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    await vm.LoadData();
                }
                else
                {
                    await vm.FilterSources(searchText);
                }
                Debug.WriteLine("Performing search operation: " + searchText);

                // Reset the debounce timer and other related flags
                debounceTimer.Dispose();
                debounceTimer = null;
            });
        }
    }
}