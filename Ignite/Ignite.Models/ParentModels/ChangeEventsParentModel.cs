using Ignite.Models.InputModels.Events;
using Ignite.Models.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class ChangeEventsParentModel
    {
        public ChangeEventViewModel ViewModel { get; set; }
        public ChangeEventInputModel InputModel { get; set; }
    }
}
