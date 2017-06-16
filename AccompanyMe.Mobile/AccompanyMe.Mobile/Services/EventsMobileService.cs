using System;
using Microsoft.WindowsAzure.MobileServices;
using AccompanyMe.Mobile.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Newtonsoft.Json.Linq;
using System.Net;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AccompanyMe.Mobile.Services
{
    public class EventsMobileService
    {
        private MobileServiceClient _client;
        private IMobileServiceSyncTable<Event> _eventItemTable;
        private static EventsMobileService _instance;

        public static EventsMobileService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventsMobileService();

                return _instance;
            }
        }

		public MobileServiceClient Client
		{
			get { return _client; }
		}

        public EventsMobileService()
        {
             //_client = new MobileServiceClient(GlobalSettings.AzureEndpoint);
        }

        public async Task InitializeAsync()
        {
            if (_client != null)
                return;

            // Inicialización de SQLite local
            var store = new MobileServiceSQLiteStore("accompanyme.db");
            store.DefineTable<Event>();

            _client = new MobileServiceClient(GlobalSettings.AzureEndpoint);
            _eventItemTable = _client.GetSyncTable<Event>();

            //Inicializa utilizando IMobileServiceSyncHandler.
            await _client.SyncContext.InitializeAsync(store,
                new MobileServiceSyncHandler());

            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    // Subir cambios a la base de datos remota
                    await _client.SyncContext.PushAsync();

                    await _eventItemTable.PullAsync("allEvents", _eventItemTable.CreateQuery());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: {0}", ex.Message);
                }
            }
        }

        public async Task<IEnumerable<Event>> ReadEventsItemsAsync()
        {
            await InitializeAsync();
            await SynchronizeAsync();
            return await _eventItemTable.ReadAsync();
        }

        public async Task AddOrUpdateEventItemAsync(Event eventItem)
        {
            await InitializeAsync();

            if (string.IsNullOrEmpty(eventItem.Id))
                await _eventItemTable.InsertAsync(eventItem);
            else
                await _eventItemTable.UpdateAsync(eventItem);

            await SynchronizeAsync();
        }

        public async Task DeleteEventItemAsync(Event eventItem)
        {
            await InitializeAsync();

            await _eventItemTable.DeleteAsync(eventItem);

            await SynchronizeAsync();
        }

        private async Task SynchronizeAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return;

            try
            {
                // Subir cambios a la base de datos remota
                await _client.SyncContext.PushAsync();

                // El primer parámetro es el nombre de la query utilizada intermanente por el client SDK para implementar sync.
                // Utiliza uno diferente por cada query en la App
                await _eventItemTable.PullAsync($"all{nameof(Event)}", _eventItemTable.CreateQuery());
            }
            catch (MobileServicePushFailedException ex)
            {
                if (ex.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                {
                    await LoginAsync();
                    await SynchronizeAsync();
                    return;
                }

                if (ex.PushResult != null)
                    foreach (var result in ex.PushResult.Errors)
                        await ResolveErrorAsync(result);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await LoginAsync();
                    await SynchronizeAsync();
                    return;
                }

                throw;
            }
        }

        private async Task ResolveErrorAsync(MobileServiceTableOperationError result)
        {
            // Ignoramos si no podemos validar ambas partes.
            if (result.Result == null || result.Item == null)
                return;

            var serverItem = result.Result.ToObject<Event>();
            var localItem = result.Item.ToObject<Event>();

            if (serverItem.Name == localItem.Name
                && serverItem.Id == localItem.Id)
            {
                // Los elementos son iguales, ignoramos el conflicto
                await result.CancelAndDiscardItemAsync();
            }
            else
            {
                // El cliente manda.
                localItem.AzureVersion = serverItem.AzureVersion;
                await result.UpdateOperationAsync(JObject.FromObject(localItem));
            }
        }

        public async Task LoginAsync()
        {
            const string userIdKey = ":UserId";
            const string tokenKey = ":Token";

            if (CrossSecureStorage.Current.HasKey(userIdKey)
                && CrossSecureStorage.Current.HasKey(tokenKey))
            {
                string userId = CrossSecureStorage.Current.GetValue(userIdKey);
                string token = CrossSecureStorage.Current.GetValue(tokenKey);

                _client.CurrentUser = new MobileServiceUser(userId)
                {
                    MobileServiceAuthenticationToken = token
                };

                return;
            }

            var authService = DependencyService.Get<IAuthService>();
            await authService.LoginAsync(_client, MobileServiceAuthenticationProvider.Facebook);

            var user = _client.CurrentUser;

            if (user != null)
            {
                CrossSecureStorage.Current.SetValue(userIdKey, user.UserId);
                CrossSecureStorage.Current.SetValue(tokenKey, user.MobileServiceAuthenticationToken);
            }
        }
    }
}