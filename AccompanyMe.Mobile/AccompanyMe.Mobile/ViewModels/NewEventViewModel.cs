using System;
using System.Diagnostics;
using System.Windows.Input;
using AccompanyMe.Mobile.Model;
using AccompanyMe.Mobile.Services;
using AccompanyMe.Mobile.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AccompanyMe.Mobile.ViewModels
{
    public class NewEventViewModel : ViewModelBase
    {
        private string _name;
        private string _description;
        private DateTime _date;
        private double _latitude;
        private double _longitude;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        public double Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        public override void OnAppearing(object navigationContext)
        {
            try
            {
                var position = (Position)navigationContext;

                Date = DateTime.Now;
                this._latitude = position.Latitude;
                this._longitude = position.Longitude;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand SaveCommand => new Command(SaveAsync);

        public ICommand CancelCommand => new Command(Cancel);

        private async void SaveAsync()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var eventItem = new Event
                {
                    Name = _name,
                    Description = _description,
                    Date = _date,
                    Latitude = _latitude,
                    Longitude = _longitude

                };

                await EventsMobileService.Instance.AddOrUpdateEventItemAsync(eventItem);
            }

            NavigationService.Instance.NavigateBack();
        }

        private void Cancel()
        {
            NavigationService.Instance.NavigateBack();
        }
    }
}
