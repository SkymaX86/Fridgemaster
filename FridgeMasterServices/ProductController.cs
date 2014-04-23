using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FridgeMasterServices
{
    public class ProductController : ApiController
    {
        /// <summary>
        /// Static list to simulate a database - will be reset to constructor defaultvalues every time the web restarts
        /// </summary>
        private static List<Product> productList = new List<Product>();
        
        static ProductController()
        {
            var product1 = new Product() { Id = 0, Name = "Butter", Einheit = 0, Menge = 200 };
            var product2 = new Product() { Id = 1, Name = "Bread", Einheit = 1, Menge = 1 };
            var product3 = new Product() { Id = 2, Name = "Tomato", Einheit = 2, Menge = 2 };
            var product4 = new Product() { Id = 3, Name = "Potato", Einheit = 3, Menge = 4 };

            productList.Add(product1);
            productList.Add(product2);
            productList.Add(product3);
            productList.Add(product4);
        }

        /// <summary>
        /// Using the GET-Verb attribute to get the productlist
        /// Route : .../api/{controller}/{action}"
        /// Sample: .../api/product/list
        /// </summary>
        /// <returns>A static list of products</returns>
        [HttpGet]
        [ActionName("list")]
        public List<Product> GetProducts()
        {
            return productList;
        }

        /// <summary>
        /// Using the GET-Verb attribute to get one product
        /// Route : .../api/{controller}/{action}/{Id}"
        /// Sample: .../api/product/single/1
        /// </summary>
        /// <param name="id">The product id</param>
        [HttpGet]
        [ActionName("single")]
        public Product GetProduct(int id)
        {
            var result = productList.FirstOrDefault(p => p.Id == id);

            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Using the DELETE-Verb attribute to delete a product
        /// *Maybe you must disable verbfiltering for this web or virtual directory on IIS WebDav if present
        /// - have a look at the web.config in my WebHost project*
        /// Route : .../api/{controller}/{action}/{Id}"
        /// Sample: .../api/product/single/1
        /// </summary>
        /// <param name="id">The product id</param>
        [HttpDelete]
        [ActionName("single")]
        public void DeleteProduct(int id)
        {
            productList.RemoveAll(p => p.Id == id);
        }

        /// <summary>
        /// Using the Post-Verb attribute to add/edit a product
        /// Route : .../api/{controller}/{action}"
        /// Sample: .../api/product/list
        /// </summary>
        /// <param name="product">The product</param>
        [HttpPost]
        [ActionName("list")]
        public void AddEditProduct(Product product)
        {
            if (product.Id == -1)
            {
                int maxId = productList.Max(p => p.Id);

                product.Id = maxId + 1;
                productList.Add(product);
            }
            else
            {
                DeleteProduct(product.Id);
                productList.Add(product);
            }
        }
    }
}
