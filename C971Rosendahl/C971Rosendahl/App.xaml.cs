using C971Rosendahl.Models;
using C971Rosendahl.Services;
using C971Rosendahl.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Rosendahl
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            var degreePlan = new DegreePlan();
            var navPage = new NavigationPage(degreePlan);
            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
