using Ignite.Models.InputModels.Events;
using Ignite.Models.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class AllEventsParentModel
    {
        public AddEventInputModel AddEventInputModel { get; set; }

        public List<ShowEventsViewModel>ShowEventsViewModel { get; set; }
    }
}
