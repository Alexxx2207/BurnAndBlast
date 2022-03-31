using Ignite.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ViewModels.Products
{
    public class ProductInCartViewModel
    {
        public string GUID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ExpirationDate { get; set; }

        public ProductType ProductType { get; set; }
    }
}
