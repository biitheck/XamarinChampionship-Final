using System;
using System.Collections.Generic;
using AccompanyMe.Mobile.ViewModels;
using Xamarin.Forms;

namespace AccompanyMe.Mobile.Views
{
    public partial class NewEventView : ContentPage
    {
        public object Parameter { get; set; }

        public NewEventView(object parameter)
        {
            InitializeComponent();

            Parameter = parameter;
            BindingContext = new NewEventViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NewEventViewModel viewModel = (NewEventViewModel)BindingContext;

            if (viewModel != null)
                viewModel.OnAppearing(Parameter);
        }
    }
}
