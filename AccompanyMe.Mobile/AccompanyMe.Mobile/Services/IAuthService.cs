using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace AccompanyMe.Mobile.Services
{
    public interface IAuthService
    {
        Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
    }
}
