using Ignite.Models;
using Ignite.Models.InputModels.Events;
using Ignite.Models.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Events
{
    public interface IEventsService
    {
        List<ShowEventsViewModel> GetEvents(string userId);

        void AddEvent(AddEventInputModel model);

        void RemoveEvent(string userId, string eventId);

        bool CheckEventExists(string eventId);

        void AddUserToEvent(string userId, string eventId);

        void RemoveUserFromEvent(string userId, string eventId);

        Event GetEventByGUID(string eventId);

        void ChangeEvent(Models.InputModels.Events.ChangeEventInputModel model);

        ShowEventDetailsViewModel GetDetailsOfEvent(string userId, string eventId);

        bool IsNameAvailable(string name, string guid);

        public List<ShowEventsViewModel> GetTopEvents(string userId, int count);
    }
}
