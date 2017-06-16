using System;
using Xamarin.Forms;

namespace AccompanyMe.Mobile.ViewModels.Base
{
    public class ViewModelBase : BindableObject
    {
		private bool _isBusy;

		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				OnPropertyChanged();
			}
		}

        public virtual void OnAppearing(object navigationContext)
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }
}
