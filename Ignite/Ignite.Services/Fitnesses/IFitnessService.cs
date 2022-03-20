using Ignite.Models.InputModels.Fitnesses;
using Ignite.Models.ViewModels.Fitnesses;

namespace Ignite.Services.Fitnesses
{
    public interface IFitnessService
    {
        void AddFitness(FitnessesInputModel model);

        void RemoveFitness(string fitnessId);
        
        List<GetFitnessViewModel> GetAllFitnesses();

        bool CheckFitnessExist(string fitnessId);

        bool IsNameAvailable(string name);
    }
}
