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

        void RemoveEvent(string eventId);

        bool CheckEventExists(string eventId);
    }
}
