﻿using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Fitnesses;
using Ignite.Models.ViewModels.Fitnesses;
using Microsoft.AspNetCore.Http;

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

        public bool CheckFitnessExist(string fitnessId)
        {
            return db.Fitnesses.Any(x => x.Guid == fitnessId);
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

        public bool IsNameAvailable(string name)
        {
            return !db.Fitnesses.Any(f => f.Name.ToLower() == name.ToLower() && !f.IsDeleted);
        }

        public void RemoveFitness(string fitnessId)
        {
            var fitness = db.Fitnesses.Find(fitnessId);

            fitness.IsDeleted = true;

            db.SaveChanges();
        }
    }
}
