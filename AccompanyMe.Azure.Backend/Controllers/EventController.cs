using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using AccompanyMe.Azure.Backend.DataObjects;
using AccompanyMe.Azure.Backend.Models;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.Mobile.Server.Config;
using System.Collections.Generic;

namespace AccompanyMe.Azure.Backend.Controllers
{
    public class EventController : TableController<Event>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Event>(context, Request);
        }

        // GET tables/Event
        public IQueryable<Event> GetAllEvent()
        {
            return Query(); 
        }

        // GET tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Event> GetEvent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Event> PatchEvent(string id, Delta<Event> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Event
        public async Task<IHttpActionResult> PostEvent(Event item)
        {
            Event current = await InsertAsync(item);

            // Obtener la configuracion para el proyecto server.
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings = Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Obtener las credenciales del Notification Hubs para la Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Crear el cliente Notification Hub.
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // APNS, GCM, WNS and MPNS templates.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = item.Name + " ah sido añadido.";

            try
            {
                // Enviar la notificacion push.
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                // Escribir el resultado en los logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Escribir el resultado del fallo en los logs.
                config.Services.GetTraceWriter().Error(ex.Message, null, "Push.SendAsync Error");
            }

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEvent(string id)
        {
             return DeleteAsync(id);
        }
    }
}
