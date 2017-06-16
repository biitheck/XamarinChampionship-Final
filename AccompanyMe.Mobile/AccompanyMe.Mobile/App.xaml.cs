using System;
using AccompanyMe.Mobile.Model;
using AccompanyMe.Mobile.Services;
using AccompanyMe.Mobile.Views;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using Xamarin.Forms;

namespace AccompanyMe.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            GmsPlace.Init("AIzaSyDIgc3KeHRMQko6Uviq-8ImCdRclzG2v0I");
            GmsDirection.Init("AIzaSyDIgc3KeHRMQko6Uviq-8ImCdRclzG2v0I");

            MainPage = new NavigationPage(new MainView(null));
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await EventsMobileService.Instance.InitializeAsync();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            TKNativePlacesApi.Instance.DisconnectAndRelease();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
