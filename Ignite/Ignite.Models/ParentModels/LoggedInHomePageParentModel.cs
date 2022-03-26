using Ignite.Models.ViewModels.Classes;
using Ignite.Models.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class LoggedInHomePageParentModel
    {
        public List<ShowEventsViewModel> TopEvents { get; set; }
        public List<AllClassesViewModel> TopClasses { get; set; }
    }
}
