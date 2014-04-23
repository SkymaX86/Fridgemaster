using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

///Because of crossdomain call issues, I must setup Web and Host Project to the same root domain on my local development IIS to make this work.

[assembly: OwinStartup(typeof(FridgeMasterWebHost.Startup))]
namespace FridgeMasterWebHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Mapping the Hubs. In this Sample I use the generated javascript
            //You can have a look at the generated javascriptcode here: .../WebHost/signalr/hubs
            app.MapSignalR();
        }
    }
}
