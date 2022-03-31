using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Classes;
using Ignite.Models.ViewModels.Classes;
using Ignite.Services.Products;
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
        private readonly IProductsService productsService;

        public ClassesService(
            ApplicationDbContext db,
            IProductsService productsService)
        {
            this.db = db;
            this.productsService = productsService;
        }

        public void AddClasses(AddClassInputModel model)
        {
            var guid = Guid.NewGuid().ToString();

            var classs = new Class
            {
                Guid = guid,
                Name = model.Name,
                Address = model.Address,
                Description = model.Description,
                DurationInMinutes = model.DurationInMinutes.Value,
                AllSeats = model.AllSeats.Value,
                StartingDateTime = model.StartingDateTime.Value,
            };

            var product = new Product()
            {
                Guid = guid,
                Name = model.Name,
                Price = model.Price.Value,
                ProductType = Models.Enums.ProductType.Class
            };
            
            db.Classes.Add(classs);
            db.Products.Add(product);

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

            var p = productsService.GetProductByGUID(model.Guid);

            p.Name = model.Name;
            p.Price = model.Price.Value;

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

            var product = productsService.GetProductByGUID(classId);

            return new ShowClassDetailsViewModel
            {
                Guid = classs.Guid,
                Name = classs.Name,
                Address = classs.Address,
                Price = product.Price.ToString("f2"),
                StartingDateTime = classs.StartingDateTime,
                Description = classs.Description,
                AllSeats = classs.AllSeats,
                DurationInMinutes = classs.DurationInMinutes,
                UserAttends = classs.UsersClasses.Any(ue => ue.UserId == userId),
                UsersCount = classs.UsersClasses.Count
            };
        }

        public void RemoveClass(string userId, string classId)
        {
            var userClasses = db.UsersClasses.Where(ue => ue.ClassId == classId).ToList();
            var userProducts = db.UsersProducts.Where(up => up.ProductId == classId).ToList();

            db.UsersClasses.RemoveRange(userClasses);
            db.UsersProducts.RemoveRange(userProducts);

            GetClassByGUID(classId).IsDeleted = true;
            productsService.GetProductByGUID(classId).IsDeleted = true;

            db.SaveChanges();
        }

        public bool IsNameAvailable(string name, string guid)
        {
            return !db.Classes.Any(f => f.Name.ToLower() == name.ToLower() && 
                        !f.IsDeleted && (guid == null || f.Guid != guid));
        }

        public List<AllClassesViewModel> GetTopClasses(string userId, int count)
        {
            return db.Classes
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.UsersClasses.Count)
                .Take(count)
                .Select(c => new AllClassesViewModel
                {
                    Guid = c.Guid,
                    Name = c.Name,
                    Address = c.Address,
                    AllSeats = c.AllSeats,
                    DurationInMinutes = c.DurationInMinutes,
                    StartingDateTime = c.StartingDateTime,
                    UserAttends = c.UsersClasses.Any(ue => ue.UserId == userId),
                    UsersCount = c.UsersClasses.Count
                })
                .ToList();
        }
    }
}
