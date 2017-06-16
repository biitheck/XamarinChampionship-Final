using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AccompanyMe.Mobile.Model;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AccompanyMe.Mobile.Services
{
    public class EventsServices
    {
        private HttpClient _client;
        private static EventsServices _instance;

        public EventsServices()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            _client.MaxResponseContentBufferSize = 256000;
        }

        public static EventsServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventsServices();

                return _instance;
            }
        }

        public async Task<List<Event>> GetItems()
        {
            var items = new List<Event>();
            var uri = new Uri(string.Format("{0}/tables/events", GlobalSettings.AzureEndpoint));

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<List<Event>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }

            return items;
        }
    }
}
