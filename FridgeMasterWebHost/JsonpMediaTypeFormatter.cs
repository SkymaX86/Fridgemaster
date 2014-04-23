using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SkymaX.Web.Http.Formatters
{
    /// <summary>
    /// Custom Mediatypeformatter for Jsonp, because I've some troubles with crossdomain-calls.
    /// But it turned out, that only the GET-Verb functions work with crossdomain-calls.
    /// So this class is fine if you just wanna get data from the server.
    /// 
    /// Implementation Sample for Global.asax (implements exactly like any other mediatypeformaters)
    /// --------------------------------------------------------------------------------------------
    /// var formatterJsonp = new JsonpMediaTypeFormatter();
    /// config.Formatters.Clear();
    /// config.Formatters.Add(formatterJsonp);
    /// 
    /// Another alternative is the JsonpMediaTypeFormatter from "WebApiContrib"
    /// PM> Install-Package WebApiContrib.Formatting.Jsonp
    /// </summary>
    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private string callbackQueryParameter;

        public JsonpMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(DefaultMediaType);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", DefaultMediaType));
        }

        public string CallbackQueryParameter
        {
            get { return callbackQueryParameter ?? "callback"; }
            set { callbackQueryParameter = value; }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, System.Net.TransportContext transportContext)
        {
            string callback;

            if (IsJsonpRequest(out callback))
            {
                return Task.Factory.StartNew(() =>
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(callback + "(");
                    writer.Flush();

                    base.WriteToStreamAsync(type, value, stream, content, transportContext).Wait();

                    writer.Write(")");
                    writer.Flush();
                });
            }
            else
            {
                return base.WriteToStreamAsync(type, value, stream, content, transportContext);
            }
        }

        private bool IsJsonpRequest(out string callback)
        {
            callback = null;

            if (HttpContext.Current.Request.HttpMethod != "GET")
                return false;

            callback = HttpContext.Current.Request.QueryString[CallbackQueryParameter];

            return !string.IsNullOrEmpty(callback);
        }
    }
}