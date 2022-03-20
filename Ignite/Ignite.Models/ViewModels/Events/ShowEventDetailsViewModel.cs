using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ViewModels.Events
{
    public class ShowEventDetailsViewModel
    {
        public string Guid { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime StartingDateTime { get; set; }

        public string? Description { get; set; }

        public long UsersCount { get; set; }

        public bool UserAttends { get; set; }
    }
}
