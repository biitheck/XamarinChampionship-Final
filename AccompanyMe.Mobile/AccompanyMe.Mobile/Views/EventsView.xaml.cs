using AccompanyMe.Mobile.Model;
using AccompanyMe.Mobile.ViewModels;
using AccompanyMe.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccompanyMe.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsView : ContentPage
    {
        public object Parameter { get; set; }

        public EventsView(object parameter)
        {
            InitializeComponent();

            Parameter = parameter;
            BindingContext = new EventViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (EventViewModel)BindingContext;

            if (viewModel != null)
                viewModel.OnAppearing(Parameter);
        }
    }
}