using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Fitnesses;
using Ignite.Models.ViewModels.Fitnesses;

namespace Ignite.Services.Fitnesses
{
    public class FitnessService : IFitnessService
    {
        private readonly ApplicationDbContext db;

        public FitnessService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddFitness(FitnessesInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name must be provied!");
            }
            else if (string.IsNullOrWhiteSpace(model.Address))
            {
                throw new ArgumentException("Address must be provied!");
            }

            var fitness = new Fitness
            {
                Guid = Guid.NewGuid().ToString(),
                Name = model.Name,
                Address = model.Address,
                IsDeleted = false,
            };

            db.Fitnesses.Add(fitness);
            db.SaveChanges();
        }

        public List<GetFitnessViewModel> GetAllFitnesses()
        {
            return db.Fitnesses
                .Where(f => !f.IsDeleted)
                .Select(fitness => new GetFitnessViewModel
                {
                    Guid = fitness.Guid,
                    Address = fitness.Address,
                    Name = fitness.Name,
                })
                .ToList();
        }

        public void RemoveFitness(string fitnessId)
        {
            var fitness = db.Fitnesses.Find(fitnessId);

            fitness.IsDeleted = true;

            db.SaveChanges();
        }
    }
}
