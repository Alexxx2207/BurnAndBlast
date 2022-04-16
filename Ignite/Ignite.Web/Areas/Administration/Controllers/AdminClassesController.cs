﻿using Ignite.Models.InputModels.Classes;
using Ignite.Models.ParentModels;
using Ignite.Models.ViewModels.Classes;
using Ignite.Services.Classes;
using Ignite.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminClassesController : Controller
    {
        private readonly IClassesService classesService;
        private readonly IProductsService productsService;

        public AdminClassesController(
            IClassesService classesService,
            IProductsService productsService)
        {
            this.classesService = classesService;
            this.productsService = productsService;
        }

        public IActionResult AllClasses()
        {
            var model = new AllClassesParentModel
            {
                AddClassInputModel = new AddClassInputModel(),
                ShowClassesViewModel = classesService.GetAllClasses(User.FindFirstValue(ClaimTypes.NameIdentifier))
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddClass(AllClassesParentModel model)
        {
            ModelState.Remove("ShowClassesViewModel");

            if (!string.IsNullOrWhiteSpace(model.AddClassInputModel.Name) &&
                !classesService.IsNameAvailable(model.AddClassInputModel.Name, null))
            {
                ModelState.AddModelError("nameExists", $"Class with name '{model.AddClassInputModel.Name}' already exists!");
            }
            if (model.AddClassInputModel.StartingDateTime == null)
            {
                ModelState.AddModelError("dateMissing", $"The Start Date & Time field is required.");
                ModelState.Remove("AddClassInputModel.StartingDateTime");
            }

            if (!ModelState.IsValid)
            {
                var parentModel = new AllClassesParentModel()
                {
                    AddClassInputModel = model.AddClassInputModel,
                    ShowClassesViewModel = classesService.GetAllClasses(User.FindFirstValue(ClaimTypes.NameIdentifier))
                };

                return View("AllClasses", parentModel);

            }

            classesService.AddClasses(model.AddClassInputModel);

            return Redirect("/Classes/All");
        }

        public IActionResult RemoveClass(string classId)
        {
            if (classesService.CheckClassExists(classId))
                classesService.RemoveClass(classId);

            return Redirect("/Administration/AdminClasses/AllClasses");
        }

        public IActionResult ChangeClass(string classId)
        {
            var ev = classesService.GetClassByGUID(classId);

            var model = new ChangeClassesParentModel
            {
                InputModel = new ChangeClassInputModel(),
                ViewModel = new ChangeClassViewModel
                {
                    Guid = classId,
                    Address = ev.Address,
                    Name = ev.Name,
                    StartingDateTime = ev.StartingDateTime,
                    Description = ev.Description,
                    AllSeats = ev.AllSeats, 
                    DurationInMinutes = ev.DurationInMinutes,   
                    Price = productsService.GetProductByGUID(classId).Price.ToString("f2")
                }
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangeClass(ChangeClassesParentModel model)
        {
            ModelState.Remove("ViewModel");

            if (!string.IsNullOrWhiteSpace(model.InputModel.Name) &&
                !classesService.IsNameAvailable(model.InputModel.Name, model.InputModel.Guid))
            {
                ModelState.AddModelError("nameExists", $"Class with name '{model.InputModel.Name}' already exists!");
            }

            if (!ModelState.IsValid)
            {
                var ev = classesService.GetClassByGUID(model.InputModel.Guid);

                model.ViewModel = new ChangeClassViewModel
                {
                    Guid = model.InputModel.Guid,
                    Address = ev.Address,
                    Name = ev.Name,
                    StartingDateTime = ev.StartingDateTime,
                    Description = ev?.Description,
                    AllSeats = ev.AllSeats, 
                    DurationInMinutes= ev.DurationInMinutes,
                    Price = productsService.GetProductByGUID(model.InputModel.Guid).Price.ToString("f2")
                };

                return View("ChangeClass", model);
            }

            classesService.ChangeClass(model.InputModel);


            return Redirect("/Administration/AdminClasses/AllClasses");
        }
    }
}
