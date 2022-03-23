using Ignite.Models.InputModels.Classes;
using Ignite.Models.ViewModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class AllClassesParentModel
    {
        public AddClassInputModel AddClassInputModel { get; set; }

        public List<AllClassesViewModel> ShowClassesViewModel { get; set; }
    }
}
