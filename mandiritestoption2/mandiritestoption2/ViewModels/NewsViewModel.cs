﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cimbtestoption2.Core;
using MvvmHelpers;
using News.Code;
using News.Helpers;
using News.Models.Responses;
using Xamarin.Forms;

namespace News.ViewModels
{
    /// <summary>
    /// News view model.
    /// </summary>
    public class NewsViewModel : BaseViewModel
    {
        public System.Collections.ObjectModel.ObservableCollection<ArticleModel> ArticleList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:News.ViewModels.NewsViewModel"/> class.
        /// </summary>
        ///
        String _categorySelected;
        String _countrySelected;
        public NewsViewModel(string categorySelected,string countrySelected)
        {
            _categorySelected = categorySelected;
            _countrySelected = countrySelected;
            LoadData();
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        public async Task LoadData()
        {
            ShowEmpty = true;
            ShowListView = false;
            IsBusy = true;

            ApiClient client = new ApiClient();
            NewsResponse news = await client.GetNews(_countrySelected,_categorySelected, Constants.ApiKey);

            if (news != null && news.Status.ToLower() == "ok")
            {
                ArticleList = new ObservableCollection<ArticleModel>(news.Articles);
                
                if (ArticleList.Count > 0)
                {
                    ShowEmpty = false;
                    ShowListView = true;
                }
                OnPropertyChanged(nameof(ArticleList));
            }

            ChangeLastUpdate();
            IsBusy = false;
        }
        public async Task FilterSources(string searchText)
        {

            ShowEmpty = true;
            ShowListView = false;
            IsBusy = true;

            ApiClient client = new ApiClient();
            NewsResponse news = await client.GetNews(_countrySelected,_categorySelected, Constants.ApiKey);
            if (news != null && news.Status.ToLower() == "ok")
            {
                ArticleList = new ObservableCollection<ArticleModel>(news.Articles);
                string lowerSearchText = searchText.ToLowerInvariant();
                ArticleList = new ObservableCollection<ArticleModel>(

                    ArticleList.Where(data => data.Title.ToLowerInvariant().Contains(lowerSearchText)));
                if (ArticleList.Count > 0)
                {
                    ShowEmpty = false;
                    ShowListView = true;
                }
                OnPropertyChanged(nameof(ArticleList));
            }

            IsBusy = false;
        }
        #region Models
        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>The last update.</value>
        public string LastUpdate
        {
            get => _lastUpdate;
            set
            {
                if (_lastUpdate != value)
                {
                    _lastUpdate = value;
                    OnPropertyChanged(nameof(LastUpdate));
                }
            }
        }
        string _lastUpdate;

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
        #region Commands
        ICommand _refreshCommand = null;

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand =
                                          new Command(async (object obj) => await ExecuteRefreshCommand()));
            }
        }

        async Task ExecuteRefreshCommand()
        {
            await LoadData();
        }
        #endregion
        #region Functions
        /// <summary>
        /// Changes the last update.
        /// </summary>
        void ChangeLastUpdate() {
            LastUpdate = "Last update at " + DateTime.Now.ToString("dd/MM/yy @ HH:mm");
        }
        #endregion
    }
}
