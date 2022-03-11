using Ignite.Models.InputModels.Fitnesses;
using Ignite.Models.ViewModels.Fitnesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class AllFitnessParentModel
    {
        public List<GetFitnessViewModel> GetFitnessViewModels { get; set; }

        public FitnessesInputModel FitnessesInputModel { get; set; }
    }
}
