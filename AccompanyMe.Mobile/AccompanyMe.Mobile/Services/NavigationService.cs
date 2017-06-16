using System;
using System.Collections.Generic;
using AccompanyMe.Mobile.ViewModels;
using AccompanyMe.Mobile.Views;
using Xamarin.Forms;

namespace AccompanyMe.Mobile.Services
{
    public class NavigationService
    {
        private static NavigationService _instance;
        private IDictionary<Type, Type> viewModelRouting = new Dictionary<Type, Type>()
        {
            { typeof(NewEventViewModel), typeof(NewEventView) },
            { typeof(EventViewModel), typeof(EventsView) },
            { typeof(EventDetailViewModel), typeof(EventDetailView) }
        };

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NavigationService();
                }

                return _instance;
            }
        }

        public void NavigateTo<TDestinationViewModel>(object navigationContext = null)
        {
            try
            {
                Type pageType = viewModelRouting[typeof(TDestinationViewModel)];
                var page = (Page)Activator.CreateInstance(pageType, navigationContext);

                if (page != null)
                    Application.Current.MainPage.Navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        public void NavigateBack()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
