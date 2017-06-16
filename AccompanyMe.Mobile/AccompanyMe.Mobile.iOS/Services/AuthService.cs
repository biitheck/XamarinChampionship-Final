using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using AccompanyMe.Mobile.Services;
using AccompanyMe.Mobile.iOS.Services;

[assembly: Dependency(typeof(AuthService))]
namespace AccompanyMe.Mobile.iOS.Services
{
	public class AuthService : IAuthService
	{
		public Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
		{
			return client.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, provider);
		}
	}
}
