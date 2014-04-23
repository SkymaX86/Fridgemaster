using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeMasterServices
{
    /// <summary>
    /// The Product-Hub for SignalR
    /// Hub would be created in FridgeMasterWebHost->Startup.cs
    /// </summary>
    public class ProductHub : Hub
    {
        /// <summary>
        /// Just give the Signal to reload the products.
        /// </summary>
        public void ProductsChanged()
        {
            Clients.All.reloadProducts();
        }
    }
}
