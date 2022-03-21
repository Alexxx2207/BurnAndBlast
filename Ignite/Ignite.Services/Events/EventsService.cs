﻿using Ignite.Data;
using Ignite.Infrastructure.CustomAttributes;
using Ignite.Models;
using Ignite.Models.InputModels.Events;
using Ignite.Models.ViewModels.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Events
{
    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext db;

        public EventsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddEvent(AddEventInputModel model)
        {
            var ev = new Event()
            {
                Guid = Guid.NewGuid().ToString(),
                Name = model.Name,
                Address = model.Address,
                StartingDateTime = model.StartingDateTime.Value,
                Description = model.Description,
            };

            db.Events.Add(ev);
            db.SaveChanges();
        }

        public void AddUserToEvent(string userId, string eventId)
        {
            if (!db.Users.Any(u => u.Id == userId) || !CheckEventExists(eventId))
            {
                throw new ArgumentException("Invalid data.");
            }

            db.UsersEvents.Add(new UserEvent
            {
                EventId = eventId,
                UserId = userId
            });

            db.SaveChanges();
        }

        public bool CheckEventExists(string eventId)
        { 
            return db.Events.Any(x => x.Guid == eventId);
        }

        public List<ShowEventsViewModel> GetEvents(string userId)
        {
            return db.Events
                .Where(e => !e.IsDeleted)
                .Select(e => new ShowEventsViewModel
                {
                    Guid = e.Guid,
                    Name = e.Name,
                    Address = e.Address,
                    StartingDateTime = e.StartingDateTime,
                    UserAttends = e.UsersEvents.Any(ue => ue.UserId == userId),
                    UsersCount = e.UsersEvents.Count
                })
                .ToList();
        }

        public void RemoveEvent(string userId, string eventId)
        {
            var userEvents = db.UsersEvents.Where(ue => ue.UserId == userId && ue.EventId == eventId).ToList();

            db.UsersEvents.RemoveRange(userEvents);

            GetEventByGUID(eventId).IsDeleted = true;
            db.SaveChanges();
        }

        public void RemoveUserFromEvent(string userId, string eventId)
        {
            if (!db.Users.Any(u => u.Id == userId) || !CheckEventExists(eventId))
            {
                throw new ArgumentException("Invalid data.");
            }

            db.UsersEvents.Remove(db.UsersEvents.First(ue => ue.EventId == eventId && ue.UserId == userId));

            db.SaveChanges();
        }

        public void ChangeEvent(Models.InputModels.Events.ChangeEventInputModel model)
        {
            var ev = GetEventByGUID(model.Guid);

            ev.Name = model.Name;
            ev.Address = model.Address;
            ev.StartingDateTime = model.StartingDateTime.Value;
            ev.Description = model.Description;

            db.SaveChanges();
        }

        public Event GetEventByGUID(string eventId)
        {
            if(!CheckEventExists(eventId))
                throw new ArgumentException("Invalid data.");

            return db.Events.First(e => e.Guid == eventId);
        }

        public ShowEventDetailsViewModel GetDetailsOfEvent(string userId, string eventId)
        {
           var ev = db.Events
               .Include(e => e.UsersEvents)
               .First(e => e.Guid == eventId);

               return  new ShowEventDetailsViewModel
               {
                   Guid = ev.Guid,
                   Name = ev.Name,
                   Address = ev.Address,
                   StartingDateTime = ev.StartingDateTime,
                   Description = ev.Description,
                   UserAttends = ev.UsersEvents.Any(ue => ue.UserId == userId),
                   UsersCount = ev.UsersEvents.Count
               };
        }

        public bool IsNameAvailable(string name)
        {
            return !db.Events.Any(f => f.Name.ToLower() == name.ToLower() && !f.IsDeleted);
        }
    }
}
