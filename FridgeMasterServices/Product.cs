using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeMasterServices
{
    /// <summary>
    /// ProductModel
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Einheit { get; set; }

        public int Menge { get; set; }
    }
}
