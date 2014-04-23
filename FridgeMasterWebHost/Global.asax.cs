using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace FridgeMasterWebHost
{
    /// <summary>
    /// Very simple WebHost configuration
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var config = GlobalConfiguration.Configuration;
            
            //Initialize a new JsonMediaTypeFormatter
            var formatterJson = new JsonMediaTypeFormatter();

            //Clear all other formatters
            config.Formatters.Clear();

            //In this sample, we just want Json
            config.Formatters.Add(formatterJson);

            //Configure the http-routes - more information about calling the urls in the FridgeMasterServices->Productcontroller
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
        }
    }
}