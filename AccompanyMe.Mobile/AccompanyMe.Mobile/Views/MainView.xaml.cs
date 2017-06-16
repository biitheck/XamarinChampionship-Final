using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using AccompanyMe.Mobile.ViewModels;
using Plugin.Connectivity.Abstractions;
using Plugin.Geolocator;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AccompanyMe.Mobile.Views
{
    public partial class MainView : ContentPage
    {
        public object Parameter { get; set; }
        protected Position position;

        public MainView(object parameter)
        {
            InitializeComponent();

            MapInit();
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
            BindingContext = new MapViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (MapViewModel)BindingContext;

            if (viewModel != null)
            {
                viewModel.OnAppearing(Parameter);
            }

        }

        protected void MapInit()
        {
            //var autoComplete = new PlacesAutoComplete { ApiToUse = PlacesAutoComplete.PlacesApi.Native };
            //autoComplete.SetBinding(PlacesAutoComplete.PlaceSelectedCommandProperty, "PlaceSelectedCommand");

            var myCustomMap = new TKCustomMap(); //(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)).WithZoom(8));

            myCustomMap.SetBinding(TKCustomMap.CustomPinsProperty, "Pins");
            myCustomMap.SetBinding(TKCustomMap.MapClickedCommandProperty, "MapClickedCommand");
            myCustomMap.SetBinding(TKCustomMap.MapLongPressCommandProperty, "MapLongPressCommand");
            myCustomMap.SetBinding(TKCustomMap.MapCenterProperty, "MapCenter");
            myCustomMap.SetBinding(TKCustomMap.PinSelectedCommandProperty, "PinSelectedCommand");
            myCustomMap.SetBinding(TKCustomMap.SelectedPinProperty, "SelectedPin");
            myCustomMap.SetBinding(TKCustomMap.RoutesProperty, "Routes");
            myCustomMap.SetBinding(TKCustomMap.PinDragEndCommandProperty, "DragEndCommand");
            myCustomMap.SetBinding(TKCustomMap.CirclesProperty, "Circles");
            myCustomMap.SetBinding(TKCustomMap.CalloutClickedCommandProperty, "CalloutClickedCommand");
            myCustomMap.SetBinding(TKCustomMap.PolylinesProperty, "Lines");
            myCustomMap.SetBinding(TKCustomMap.PolygonsProperty, "Polygons");
            myCustomMap.SetBinding(TKCustomMap.MapRegionProperty, "MapRegion");
            myCustomMap.SetBinding(TKCustomMap.RouteClickedCommandProperty, "RouteClickedCommand");
            myCustomMap.SetBinding(TKCustomMap.RouteCalculationFinishedCommandProperty, "RouteCalculationFinishedCommand");
            myCustomMap.SetBinding(TKCustomMap.TilesUrlOptionsProperty, "TilesUrlOptions");
            myCustomMap.SetBinding(TKCustomMap.MapFunctionsProperty, "MapFunctions");
            myCustomMap.IsRegionChangeAnimated = true;
            myCustomMap.IsShowingUser = true;
            //autoComplete.SetBinding(PlacesAutoComplete.BoundsProperty, "MapRegion");

            this.Content = myCustomMap;
        }

        void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                DisplayAlert("Conexión a Internet", "No hay una conexión disponible", "Ok");
        }
    }
}
