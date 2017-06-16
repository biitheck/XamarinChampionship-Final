using System;
using System.Collections.Generic;
using AccompanyMe.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AccompanyMe.Mobile.Views
{
    public partial class EventDetailView : ContentPage
    {
        public object Parameter { get; set; }

        public EventDetailView(object parameter)
        {
            InitializeComponent();

            Parameter = parameter;

            BindingContext = new EventDetailViewModel();

			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(1)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (EventDetailViewModel)BindingContext;

            if (viewModel != null)
                viewModel.OnAppearing(Parameter);
        }
    }
}
