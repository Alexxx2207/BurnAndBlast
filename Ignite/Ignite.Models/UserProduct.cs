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
        [Key]
        public string OrderItemId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public bool IsInCart { get; set; }
    }
}
