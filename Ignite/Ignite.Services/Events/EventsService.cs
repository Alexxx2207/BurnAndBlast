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
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name must be provied!");
            }
            else if (string.IsNullOrWhiteSpace(model.Address))
            {
                throw new ArgumentException("Address must be provied!");
            }
            else if (model.StartingDateTime < DateTime.Now)
            {
                throw new ArgumentException("Invalid Date & Time!");
            }

            var ev = new Event()
            {
                Guid = Guid.NewGuid().ToString(),
                Name = model.Name,
                Address = model.Address,
                StartingDateTime = model.StartingDateTime,
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
                .Include(e => e.UsersEvents)
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

        public void RemoveEvent(string eventId)
        {
            if(!CheckEventExists(eventId))
                throw new ArgumentException("Invalid data.");

            db.Events.Remove(db.Events.Find(eventId));
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
            if (!CheckEventExists(model.Guid))
                throw new ArgumentException("Invalid data.");

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name must be provied!");
            }
            else if (string.IsNullOrWhiteSpace(model.Address))
            {
                throw new ArgumentException("Address must be provied!");
            }
            else if (model.StartingDateTime < DateTime.Now)
            {
                throw new ArgumentException("Invalid Date & Time!");
            }

            var ev = GetEventByGUID(model.Guid);

            var evProperties = ev.GetType().GetProperties();
            var modelProperties = model.GetType().GetProperties();

            foreach (var propM in modelProperties)
            {
                foreach (var propE in evProperties)
                {
                    if (propE.PropertyType == propM.PropertyType &&
                        propE.Name == propM.Name)
                    {
                        propE.SetValue(ev, propM.GetValue(model));
                    }
                }
            }

            db.SaveChanges();
        }

        public Event GetEventByGUID(string eventId)
        {
            if(!CheckEventExists(eventId))
                throw new ArgumentException("Invalid data.");

            return db.Events.First(e => e.Guid == eventId);
        }
    }
}
