using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using AccompanyMe.Mobile.Services;
using AccompanyMe.Mobile.Droid.Services;

[assembly: Dependency(typeof(AuthService))]
namespace AccompanyMe.Mobile.Droid.Services
{
	public class AuthService : IAuthService
	{
		public Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
		{
			return client.LoginAsync(Forms.Context, provider);
		}
	}
}
