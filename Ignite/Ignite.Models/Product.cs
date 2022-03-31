using Ignite.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models
{
    public class Product
    {
        [Key]
        public string Guid { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public ProductType ProductType { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserProduct> UsersProducts { get; set; }

    }
}
