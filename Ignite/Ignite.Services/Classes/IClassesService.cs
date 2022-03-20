using Ignite.Models;
using Ignite.Models.InputModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Classes
{
    public interface IClassesService
    {
        void AddClasses(AddClassInputModel model);

        //List<AllClassesViewModel> GetAllClasses();

        void RemoveClass(string classId);

        bool CheckClassExists(string classId);

        void AddUserToClass(string userId, string classId);

        Class GetClassByGUID(string classId);

        //void ChangeClass(ChangeClassInputModel model);

        //ShowClassDetailsViewModel GetDetailsOfClass(string userId, string classId);

    }
}
