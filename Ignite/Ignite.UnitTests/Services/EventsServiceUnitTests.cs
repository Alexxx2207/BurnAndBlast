using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Events;
using Ignite.Services.Events;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Ignite.UnitTests.Services
{
    public class EventsServiceUnitTests
    {
        [Fact]
        public void AddEvent_ShoudPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AddEvent_ShoudPass")
            .Options;

            AddEventInputModel model = new AddEventInputModel
            {
                Name = "name",
                Address = "address",
                Description = "description",
                StartingDateTime = DateTime.Now,
            };

            var expectedCount = 1;
            var actual = 0;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var eventsService = new EventsService(dbContext);
                eventsService.AddEvent(model);

                actual = dbContext.Events.Count();
            }

            Assert.Equal(expectedCount, actual);
        }

        [Fact]
        public void AddUserToEvent_InvalidUserId_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "AddUserToEvent_InvalidUserId_ShouldFail")
              .Options;

            var userId = Guid.NewGuid().ToString();
            var eventId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var eventsService = new EventsService(dbContext);

                Assert.Throws<ArgumentException>(() => eventsService.AddUserToEvent(userId, eventId));
            }
        }

        [Fact]
        public void AddUserToEvent_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "AddUserToEvent_ShouldPass")
           .Options;

            var userId = Guid.NewGuid().ToString();
            var eventId = Guid.NewGuid().ToString();

            var eventt = new Event
            {
                Guid = eventId,
                Name = "name",
                Address = "address",
                Description = "description",
                StartingDateTime = DateTime.Now,
                IsDeleted = false,
            };

            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "FirstName",
                LastName = "LastName",
            };

            var expectedCount = 1;
            var actualUserEventsCount = 0;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Users.Add(user);
                dbContext.Events.Add(eventt);

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);


                eventsService.AddUserToEvent(userId, eventId);

                actualUserEventsCount = dbContext.UsersEvents.Count();
            }

            Assert.Equal(expectedCount, actualUserEventsCount);
        }

        [Fact]
        public void ChangeEvent_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "ChangeEvent_ShouldPass")
           .Options;

            var eventId = Guid.NewGuid().ToString();

            var model = new ChangeEventInputModel
            {
                Guid = eventId,
                Name = "changed",
                Address = "changed",
                Description = "changed",
                StartingDateTime = DateTime.Now.AddDays(5),
            };

            var actualEvent = new Event();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = eventId,
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                eventsService.ChangeEvent(model);

                actualEvent = dbContext.Find<Event>(eventId);
            }

            Assert.True(actualEvent.Name == model.Name &&
                        actualEvent.StartingDateTime == model.StartingDateTime &&
                        actualEvent.Description == model.Description &&
                        actualEvent.Address == model.Address);
        }

        [Fact]
        public void GetEvents_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetEvents_ShouldPass")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Events.Add(new Event
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                Assert.Equal(2, eventsService.GetEvents(It.IsAny<string>()).Count());
            }
        }

        [Fact]
        public void GetEventByGUID_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetEventByGUID_ShouldFail")
                 .Options;

            var eventId = Guid.NewGuid().ToString();
            var eventIdDifferent = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                Assert.Throws<ArgumentException>(() => eventsService.GetEventByGUID(eventIdDifferent));
            }
        }

        [Fact]
        public void GetDetailsOfAnEvent_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetDetailsOfAnEvent_ShouldSucceed")
                 .Options;

            var eventId = Guid.NewGuid().ToString();

            var expected = new
            {
                Guid = eventId,
                Name = "name",
                Address = "address",
                Description = "description",
                StartingDateTime = DateTime.Now
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = expected.Guid,
                    Name = expected.Name,
                    Address = expected.Address,
                    Description = expected.Description,
                    StartingDateTime = expected.StartingDateTime,
                });


                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                var actualEvent = eventsService.GetDetailsOfEvent(It.IsAny<string>(), eventId);

                Assert.True(actualEvent.Guid == expected.Guid &&
                        actualEvent.Name == expected.Name &&
                        actualEvent.StartingDateTime == expected.StartingDateTime &&
                        actualEvent.Description == expected.Description &&
                        actualEvent.Address == expected.Address);
            }
        }

        [Fact]
        public void GetTopEvents_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetTopEvents_ShouldSucceed")
                 .Options;

            var eventIdTop1 = Guid.NewGuid().ToString();
            var eventIdTop2 = Guid.NewGuid().ToString();
            var eventIdTop3 = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = eventIdTop3,
                    Name = "top3",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Events.Add(new Event
                {
                    Guid = eventIdTop1,
                    Name = "top1",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Events.Add(new Event
                {
                    Guid = eventIdTop2,
                    Name = "top2",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventIdTop1,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventIdTop1,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventIdTop1,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventIdTop2,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventIdTop2,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventIdTop3,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                var topEvents = eventsService.GetTopEvents(It.IsAny<string>(), 3);

                Assert.True(topEvents[0].Name == "top1" &&
                            topEvents[1].Name == "top2" &&
                            topEvents[2].Name == "top3");
            }
        }

        [Fact]
        public void IsNameAvailable_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsNameAvailable_ShouldSucceed")
                 .Options;

            var eventId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = eventId,
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                Assert.True(eventsService.IsNameAvailable("nameDifferent", eventId));
                Assert.True(eventsService.IsNameAvailable("nameDifferent", Guid.NewGuid().ToString()));

                Assert.True(eventsService.IsNameAvailable("name", eventId));
                Assert.True(eventsService.IsNameAvailable("Name", eventId));

                Assert.True(!eventsService.IsNameAvailable("Name", Guid.NewGuid().ToString()));
                Assert.True(!eventsService.IsNameAvailable("name", Guid.NewGuid().ToString()));
            }
        }

        [Fact]
        public void RemoveEvent_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "RemoveEvent_ShouldSucceed")
                 .Options;

            var eventId = Guid.NewGuid().ToString();

            var expectedInDB = 1;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = eventId,
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                eventsService.RemoveEvent(eventId);

                Assert.Equal(expectedInDB, dbContext.Events.Count());
                Assert.Empty(eventsService.GetEvents(It.IsAny<string>()));
            }
        }

        [Fact]
        public void CheckUserAttends_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "CheckUserAttends_ShouldSucceed")
                 .Options;

            var eventId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = eventId,
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Users.Add(new ApplicationUser
                {
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.UsersEvents.Add(new UserEvent
                {
                    EventId = eventId,
                    UserId = userId,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                var actualEvent = eventsService.GetDetailsOfEvent(userId, eventId);

                Assert.True(actualEvent.UserAttends == true);
            }
        }

        [Fact]
        public void RemoveUserFromEvent_ShoudFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "RemoveUserFromEvent_ShoudFail")
              .Options;

            var userId = Guid.NewGuid().ToString();
            var eventId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var eventsService = new EventsService(dbContext);

                Assert.Throws<ArgumentException>(() => eventsService.RemoveUserFromEvent(userId, eventId));
            }
        }

        [Fact]
        public void RemoveUserFromEvent_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "RemoveUserFromEvent_ShouldSucceed")
              .Options;

            var userId = Guid.NewGuid().ToString();
            var eventId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.Add(new Event
                {
                    Guid = eventId,
                    Name = "name",
                    Address = "address",
                    Description = "description",
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Users.Add(new ApplicationUser
                {
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.UsersEvents.Add(new UserEvent
                { 
                    EventId = eventId,
                    UserId = userId,
                });

                dbContext.SaveChanges();

                var eventsService = new EventsService(dbContext);

                eventsService.RemoveUserFromEvent(userId, eventId);

                Assert.Empty(dbContext.UsersEvents.ToList());
            }
        }
    }
}
