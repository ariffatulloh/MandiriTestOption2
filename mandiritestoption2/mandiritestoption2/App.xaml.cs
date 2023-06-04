using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using News;

namespace mandiritestoption2
{
    public partial class App : Application
    {

        public App ()
        {
            //InitializeComponent();
            InitializeComponent();

            MainPage = new NavigationPage(new NewsSource());
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

