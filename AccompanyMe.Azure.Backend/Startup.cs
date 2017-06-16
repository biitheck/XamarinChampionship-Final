using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AccompanyMe.Azure.Backend.Startup))]

namespace AccompanyMe.Azure.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}