using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Classes;
using Ignite.Models.ViewModels.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Classes
{
    public class ClassesService : IClassesService
    {
        private readonly ApplicationDbContext db;

        public ClassesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddClasses(AddClassInputModel model)
        {
            var classs = new Class
            {
                Guid = Guid.NewGuid().ToString(),
                Name = model.Name,
                Address = model.Address,
                Description = model.Description,
                DurationInMinutes = model.DurationInMinutes.Value,
                AllSeats = model.AllSeats.Value,
                Price = model.Price.Value,
                StartingDateTime = model.StartingDateTime.Value,
            };

            db.Classes.Add(classs);
            db.SaveChanges();
        }

        public void AddUserToClass(string userId, string classId)
        {
            if (!db.Users.Any(u => u.Id == userId) || !CheckClassExists(classId))
            {
                throw new ArgumentException("Invalid data.");
            }

            db.UsersClasses.Add(new UserClass
            {
                ClassId = classId,
                UserId = userId
            });

            db.SaveChanges();
        }

        public void ChangeClass(ChangeClassInputModel model)
        {
            var c = GetClassByGUID(model.Guid);

            c.Name = model.Name;
            c.Address = model.Address;
            c.StartingDateTime = model.StartingDateTime.Value;
            c.Description = model.Description;
            c.DurationInMinutes = model.DurationInMinutes.Value;
            c.AllSeats = model.AllSeats.Value;
            c.Price = model.Price.Value;

            db.SaveChanges();
        }

        public bool CheckClassExists(string classId)
        {
            return db.Classes.Any(x => x.Guid == classId);
        }

        public List<AllClassesViewModel> GetAllClasses(string userId)
        {
            return db.Classes
                .Where(c => !c.IsDeleted)
                .Select(c => new AllClassesViewModel
                {
                    Guid = c.Guid,
                    Name = c.Name,
                    Address = c.Address,
                    Price = c.Price,
                    AllSeats = c.AllSeats,
                    DurationInMinutes = c.DurationInMinutes,
                    StartingDateTime = c.StartingDateTime,
                    UserAttends = c.UsersClasses.Any(ue => ue.UserId == userId),
                    UsersCount = c.UsersClasses.Count
                })
                .ToList();
        }

        public Class GetClassByGUID(string classId)
        {
            if (!CheckClassExists(classId))
                throw new ArgumentException("Invalid data.");

            return db.Classes.First(c => c.Guid == classId);
        }

        public ShowClassDetailsViewModel GetDetailsOfClass(string userId, string classId)
        {
            var classs = db.Classes
               .Include(c => c.UsersClasses)
               .First(c => c.Guid == classId);

            return new ShowClassDetailsViewModel
            {
                Guid = classs.Guid,
                Name = classs.Name,
                Address = classs.Address,
                StartingDateTime = classs.StartingDateTime,
                Description = classs.Description,
                Price = classs.Price,
                AllSeats = classs.AllSeats,
                DurationInMinutes = classs.DurationInMinutes,
                UserAttends = classs.UsersClasses.Any(ue => ue.UserId == userId),
                UsersCount = classs.UsersClasses.Count
            };
        }

        public void RemoveClass(string userId, string classId)
        {
            var userClasses = db.UsersClasses.Where(ue => ue.UserId == userId && ue.ClassId == classId).ToList();

            db.UsersClasses.RemoveRange(userClasses);

            GetClassByGUID(classId).IsDeleted = true;
            db.SaveChanges();
        }

        public bool IsNameAvailable(string name, string guid)
        {
            return !db.Classes.Any(f => f.Name.ToLower() == name.ToLower() && 
                        !f.IsDeleted && (guid == null || f.Guid != guid));
        }
    }
}
