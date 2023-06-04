using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cimbtestoption2.Core;
using MvvmHelpers;
using News.Code;
using News.Models.Responses;

namespace News.ViewModels
{
    public class NewsSourceViewModel : BaseViewModel
    {
        public ObservableCollection<SourceModel> SourceList { get; set; }
        private ObservableCollection<SourceModel> filteredSourceList;

        public NewsSourceViewModel()
        {
            LoadData();
        }

        public async Task LoadData()
        {
            ShowEmpty = true;
            ShowListView = false;
            IsBusy = true;

            ApiClient client = new ApiClient();
            SourceResponse sources = await client.GetSources(Constants.ApiKey);
            if (sources != null && sources.Status.ToLower() == "ok")
            {
                SourceList = new ObservableCollection<SourceModel>(sources.Sources);
                if (SourceList.Count > 0)
                {
                    ShowEmpty = false;
                    ShowListView = true;
                }
                OnPropertyChanged(nameof(SourceList));
            }

            IsBusy = false;
        }
        public async Task FilterSources(string searchText)
        {

            ShowEmpty = true;
            ShowListView = false;
            IsBusy = true;

            ApiClient client = new ApiClient();
            SourceResponse sources = await client.GetSources(Constants.ApiKey);
            if (sources != null && sources.Status.ToLower() == "ok")
            {
                SourceList = new ObservableCollection<SourceModel>(sources.Sources);
                string lowerSearchText = searchText.ToLowerInvariant();
                SourceList = new ObservableCollection<SourceModel>(
                    SourceList.Where(source => source.Name.ToLowerInvariant().Contains(lowerSearchText)));
                if (SourceList.Count > 0)
                {
                    ShowEmpty = false;
                    ShowListView = true;
                }
                OnPropertyChanged(nameof(SourceList));
            }

            IsBusy = false;
        }
        //public void FilterSources(string searchText)
        //{

        //    if (string.IsNullOrWhiteSpace(searchText))
        //    {
        //        filteredSourceList = new ObservableCollection<SourceModel>(SourceList);
        //    }
        //    else
        //    {
        //        string lowerSearchText = searchText.ToLowerInvariant();
        //        SourceList = new ObservableCollection<SourceModel>(
        //            SourceList.Where(source => source.Name.ToLowerInvariant().Contains(lowerSearchText)));
        //    }
        //    Debug.WriteLine(nameof(SourceList));
        //    OnPropertyChanged(nameof(SourceList));
        //}

        #region Models
        /// <summary>
        /// The show empty
        /// </summary>
        bool _showEmpty;

        /// <summary>
        /// The show ListView
        /// </summary>
        bool _showListView;

        /// <summary>
        /// Gets or sets a value indicating whether the list is empty
        /// </summary>
        /// <value><c>true</c> if {show empty}; otherwise, <c>false</c>.</value>
        public bool ShowEmpty
        {
            get
            {
                return _showEmpty;
            }
            set
            {
                if (_showEmpty != value)
                {
                    _showEmpty = value;
                    OnPropertyChanged(nameof(ShowEmpty));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the list is full
        /// </summary>
        /// <value><c>true</c> if {show ListView}; otherwise, <c>false</c>.</value>
        public bool ShowListView
        {
            get
            {
                return _showListView;
            }
            set
            {
                if (_showListView != value)
                {
                    _showListView = value;
                    OnPropertyChanged(nameof(ShowListView));
                }
            }
        }
        #endregion
    }
}