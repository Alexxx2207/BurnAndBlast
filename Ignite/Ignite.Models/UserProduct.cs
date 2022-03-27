using Ignite.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models
{
    public class UserProduct
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool IsInCart { get; set; }
    }
}
