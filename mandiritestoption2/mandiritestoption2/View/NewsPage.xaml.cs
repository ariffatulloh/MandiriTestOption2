using System;
using System.Diagnostics;
using System.Threading;
using News.Code;
using News.Helpers;
using News.Models.Responses;
using News.ViewModels;
using Xamarin.Forms;

namespace News
{
    public partial class NewsPage : ContentPage
    {
        private string searchText;
        private readonly TimeSpan debounceDelay = TimeSpan.FromSeconds(5);
        private Timer debounceTimer;
        NewsViewModel vm = null;
        string source;
        SourceModel sourceSelected;
        public NewsPage(SourceModel sourceModelSelected)
        {
            InitializeComponent();
            sourceSelected = sourceModelSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadViewModel();
        }

        public void LoadViewModel() {
            Title = Settings.SourceName;
            
            if (vm == null || source != Settings.Source) {
                source = Settings.Source;
                vm = new NewsViewModel(categorySelected:sourceSelected.Category,countrySelected: sourceSelected.Country);
                BindingContext = vm;
            }
        }

        async void Change_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NewsSource(), true);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
			=> ((ListView)sender).SelectedItem = null;

		async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var source = ((ListView)sender).SelectedItem as ArticleModel;
			if (source == null)
				return;

            await Navigation.PushAsync(new NewsViewPage(source.Title, source.Url), true);
		}
        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = e.NewTextValue;
            if (debounceTimer != null)
            {
                debounceTimer.Dispose();
                debounceTimer = null;
            }

            debounceTimer = new Timer(PerformSearch, null, 3000, Timeout.Infinite);
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