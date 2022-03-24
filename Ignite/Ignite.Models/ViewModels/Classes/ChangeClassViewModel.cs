using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ViewModels.Classes
{
    public class ChangeClassViewModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime StartingDateTime { get; set; }

        public int DurationInMinutes { get; set; }

        public decimal Price { get; set; }

        public int AllSeats { get; set; }

        public string Description { get; set; }
    }
}
