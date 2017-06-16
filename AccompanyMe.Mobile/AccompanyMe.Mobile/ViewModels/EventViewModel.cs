using AccompanyMe.Mobile.Model;
using AccompanyMe.Mobile.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using AccompanyMe.Mobile.ViewModels.Base;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.Specialized;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace AccompanyMe.Mobile.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private ObservableCollection<Event> _items;
        private Event _selectedItem;
        private Map _map;

        public ObservableCollection<Event> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public Event SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                // Creando un servicio de navegación.
                NavigationService.Instance.NavigateTo<EventDetailViewModel>(_selectedItem);
            }
        }

        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                OnPropertyChanged("Map");
            }
        }

        public ICommand NewCommand => new Command(New);

        public override async void OnAppearing(object navigationContext)
        {
            base.OnAppearing(navigationContext);
            try
            {
                IsBusy = true;

                var result = await EventsMobileService.Instance.ReadEventsItemsAsync();

                if (result != null)
                    Items = new ObservableCollection<Event>(result);
                else
                    Items = new ObservableCollection<Event>();

                Items.CollectionChanged += OnCollectionChanged;

                UpdatePinsOnMap(Items);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void New()
        {
            NavigationService.Instance.NavigateTo<NewEventViewModel>();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var items = e.NewItems;

            throw new System.NotImplementedException();
        }

        private void UpdatePinsOnMap(ObservableCollection<Event> events)
        {
            foreach (var e in events)
            {
                var position = new Position(e.Latitude, e.Longitude); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = e.Name,
                    Address = e.Description
                };

                _map.Pins.Add(pin);
            }
        }

        public async void MapInit(Map map)
        {
            _map = map;

            var locator = CrossGeolocator.Current;

            var position = await locator.GetPositionAsync(10000);

            _map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                              Distance.FromMiles(1)));

        }

    }
}
