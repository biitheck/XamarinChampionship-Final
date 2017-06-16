using System;
using System.Windows.Input;
using AccompanyMe.Mobile.Model;
using AccompanyMe.Mobile.ViewModels.Base;
using AccompanyMe.Mobile.Services;
using Xamarin.Forms;

namespace AccompanyMe.Mobile.ViewModels
{
    public class EventDetailViewModel : ViewModelBase
    {
        private Event _item;

        public Event Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged("Item");
            }
        }

        public EventDetailViewModel()
        {

        }

        public ICommand DeleteCommand => new Command(DeleteAsync);

        public override void OnAppearing(object navigationContext)
        {
            base.OnAppearing(navigationContext);

            if (navigationContext is Event)
                Item = (Event)navigationContext;
        }

        private async void DeleteAsync()
        {
            if (Item.Id != null)
            {
                await EventsMobileService.Instance.DeleteEventItemAsync(Item);

                NavigationService.Instance.NavigateBack();
            }
        }
    }
}
