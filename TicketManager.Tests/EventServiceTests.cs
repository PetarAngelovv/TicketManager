using Microsoft.AspNetCore.Identity;
using Moq;
using System.Globalization;
using TicketManager.Data;
using TicketManager.Data.Models;
using TicketManager.Web.ViewModels.Event;

namespace TicketManager.Tests
{
    [TestFixture]
    public class EventServiceTests : TestBase
    {

        private Mock<UserManager<IdentityUser>> GetUserManagerMock(IdentityUser user)
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            userManagerMock
                .Setup(um => um.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(um => um.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            return userManagerMock;
        }


        private EventService GetService(TicketManagerDbContext dbContext)
        {
            var user = new IdentityUser { Id = "user1", UserName = "TestUser" };
            var userManagerMock = GetUserManagerMock(user); 

            return new EventService(dbContext, userManagerMock.Object);
        }


        // ---------- CreateEventAsync ----------
        [Test]
        public async Task CreateEventAsync_AddsEventToDatabase()
        {
            var db = GetDbContext(nameof(CreateEventAsync_AddsEventToDatabase));
            var user = new IdentityUser { Id = "user1", UserName = "TestUser" };
            db.Users.Add(user);
            var category = new Category { Id = 1, Name = "Concert" };
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            var userManagerMock = GetUserManagerMock(user);
            userManagerMock.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user);

            var service = new EventService(db, userManagerMock.Object);


            var input = new EventCreateInputModel
            {
                Name = "New Event",
                Description = "Test event description",
                TicketPrice = 10,
                TotalTickets = 100,
                ImageUrl = "http://test.com/img.jpg",
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)


            };

            var result = await service.CreateEventAsync("user1", input);

            Assert.IsTrue(result);
            Assert.AreEqual(1, db.Events.Count());
            Assert.AreEqual(100, db.Tickets.Count(), "100 tickets need to be created.");
            Assert.AreEqual(1, db.Categories.Count());

        }


        [Test]
        public async Task CreateEventAsync_ReturnsFalse_WhenUserNotFound()
        {
            var db = GetDbContext(nameof(CreateEventAsync_ReturnsFalse_WhenUserNotFound));
            var category = new Category { Id = 1, Name = "Concert" };
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            var service = GetService(db);

            var input = new EventCreateInputModel
            {
                Name = "Test",
                Description = "Test event description",
                TicketPrice = 10,
                TotalTickets = 50,
                ImageUrl = null,
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            };

            var result = await service.CreateEventAsync("invalidUser", input);

            Assert.IsFalse(result);
            Assert.AreEqual(0, db.Events.Count());
        }

        // ---------- GetEventDetailsAsync ----------
        [Test]
        public async Task GetEventDetailsAsync_ReturnsCorrectEvent()
        {
            var db = GetDbContext(nameof(GetEventDetailsAsync_ReturnsCorrectEvent));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "Test Event",
                Description = "Test event description",
                TicketPrice = 20,
                TotalTickets = 50,
                CreatedOn = DateTime.UtcNow,
                AuthorId = "user1",
                Author = user,
                CategoryId = 1,
                Category = category
            };
            db.Events.Add(ev);
            await db.SaveChangesAsync();

            var service = GetService(db);

            var result = await service.GetEventDetailsAsync("user1", 1);

            Assert.NotNull(result);
            Assert.AreEqual("Test Event", result.Name);
            Assert.AreEqual("Concert", result.CategoryName);
        }

        [Test]
        public async Task GetEventDetailsAsync_ReturnsNull_WhenEventNotFound()
        {
            var db = GetDbContext(nameof(GetEventDetailsAsync_ReturnsNull_WhenEventNotFound));
            var service = GetService(db);

            var result = await service.GetEventDetailsAsync("user1", 99);

            Assert.IsNull(result);
        }

        // ---------- GetEventForEditingAsync ----------
        [Test]
        public async Task GetEventForEditingAsync_ReturnsCorrectEvent()
        {
            var db = GetDbContext(nameof(GetEventForEditingAsync_ReturnsCorrectEvent));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "Old Name",
                Description = "Old Test event description",
                TicketPrice = 20,
                TotalTickets = 50,
                CreatedOn = DateTime.UtcNow,
                AuthorId = "user1",
                Author = user,
                CategoryId = 1,
                Category = category
            };
            db.Events.Add(ev);
            await db.SaveChangesAsync();

            var service = GetService(db);
            var result = await service.GetEventForEditingAsync("user1", 1, isAdmin: false);

            Assert.NotNull(result);
            Assert.AreEqual("Old Name", result.Name);
        }

        [Test]
        public async Task GetEventForEditingAsync_ReturnsNull_WhenEventNotFound()
        {
            var db = GetDbContext(nameof(GetEventForEditingAsync_ReturnsNull_WhenEventNotFound));
            var service = GetService(db);

            var result = await service.GetEventForEditingAsync("user1", 99, isAdmin: false);

            Assert.IsNull(result);
        }

        // ---------- GetEventForDeletingAsync ----------
        [Test]
        public async Task GetEventForDeletingAsync_ReturnsCorrectEvent()
        {
            var db = GetDbContext(nameof(GetEventForDeletingAsync_ReturnsCorrectEvent));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "To Delete",
                Description = "Test event description",
                TicketPrice = 20,
                TotalTickets = 50,
                CreatedOn = DateTime.UtcNow,
                AuthorId = "user1",
                Author = user,
                CategoryId = 1,
                Category = category
            };
            db.Events.Add(ev);
            await db.SaveChangesAsync();

            var service = GetService(db);
            var result = await service.GetEventForDeletingAsync("user1", 1);

            Assert.NotNull(result);
            Assert.AreEqual("To Delete", result.Name);
        }

        [Test]
        public async Task GetEventForDeletingAsync_ReturnsNull_WhenEventNotFound()
        {
            var db = GetDbContext(nameof(GetEventForDeletingAsync_ReturnsNull_WhenEventNotFound));
            var service = GetService(db);

            var result = await service.GetEventForDeletingAsync("user1", 99);

            Assert.IsNull(result);
        }

        // ---------- SoftDeleteEventAsync ----------
        [Test]
        public async Task SoftDeleteEventAsync_SetsIsDeletedFlag()
        {
            var db = GetDbContext(nameof(SoftDeleteEventAsync_SetsIsDeletedFlag));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "SoftDelete",
                Description = "Test event description",
                TicketPrice = 20,
                TotalTickets = 10,
                CreatedOn = DateTime.UtcNow,
                AuthorId = user.Id,
                CategoryId = 1
            };
            db.Events.Add(ev);
            await db.SaveChangesAsync();

            var userManagerMock = GetUserManagerMock(user);
            var service = new EventService(db, userManagerMock.Object);

            var input = new EventDeleteInputModel { Id = 1, Name = "SoftDelete" };
            var result = await service.SoftDeleteEventAsync(user.Id, input);

            Assert.IsTrue(result);

            var deletedEvent = await db.Events.FindAsync(1);
            Assert.IsTrue(deletedEvent.IsDeleted);
        }




        // ---------- HardDeleteEventAsync ----------
        [Test]
        public async Task HardDeleteEventAsync_RemovesEventFromDb()
        {
            var db = GetDbContext(nameof(HardDeleteEventAsync_RemovesEventFromDb));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            db.Events.Add(new Event
            {
                Id = 1,
                Name = "HardDelete",
                Description = "Test event description",
                TicketPrice = 5,
                TotalTickets = 5,
                AuthorId = "user1",
                CategoryId = 1
            });
            await db.SaveChangesAsync();

            var service = GetService(db);
            var result = await service.HardDeleteEventAsync("user1", 1);

            Assert.IsTrue(result);
            Assert.AreEqual(0, db.Events.Count());
        }

        // ---------- PersistUpdatedEventAsync ----------
        [Test]
        public async Task PersistUpdatedEventAsync_UpdatesEventData()
        {
            var db = GetDbContext(nameof(PersistUpdatedEventAsync_UpdatesEventData));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "Old",
                Description = "Old Test event description",
                TicketPrice = 20,
                TotalTickets = 50,
                CreatedOn = DateTime.UtcNow,
                AuthorId = "user1",
                CategoryId = 1
            };
            db.Events.Add(ev);
            await db.SaveChangesAsync();

            var service = GetService(db);

            var updated = new EventEditInputModel
            {
                Id = 1,
                Name = "New",
                Description = "New Test event description",
                TicketPrice = 25,
                TotalTickets = 60,
                ImageUrl = null,
                CategoryId = 1,
                CreatedOn = ev.CreatedOn.ToString("yyyy-MM-dd")
            };


            var result = await service.PersistUpdatedEventAsync("user1", updated, isAdmin: false);

            Assert.IsTrue(result);
            Assert.AreEqual("New", db.Events.First().Name);
        }

        // ---------- Favorites ----------
        [Test]
        public async Task AddEventToUserFavoritesListAsync_AddsFavorite()
        {
            var db = GetDbContext(nameof(AddEventToUserFavoritesListAsync_AddsFavorite));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "FavEvent",
                Description = "Test event description",
                TicketPrice = 10,
                TotalTickets = 10,
                AuthorId = "user2",
                CategoryId = 1
            };
            db.Events.Add(ev);
            await db.SaveChangesAsync();

            var service = GetService(db);

            // ACT
            var result = await service.AddEventToUserFavoritesListAsync("user1", 1);

            // ASSERT
            Assert.IsTrue(result);
            Assert.AreEqual(1, db.UsersEvents.Count());

            var fav = db.UsersEvents.First();

            Assert.AreEqual("user1", fav.UserId);
            Assert.AreEqual(1, fav.EventId);
        }


        [Test]
        public async Task RemoveEventFromUserFavoritesListAsync_RemovesFavorite()
        {
            var db = GetDbContext(nameof(RemoveEventFromUserFavoritesListAsync_RemovesFavorite));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "FavEvent",
                Description = "Test event description",
                TicketPrice = 10,
                TotalTickets = 10,
                AuthorId = "user1",
                CategoryId = 1
            };
            db.Events.Add(ev);
            db.UsersEvents.Add(new UserEvent { UserId = "user1", EventId = 1 });
            await db.SaveChangesAsync();

            var service = GetService(db);

            var result = await service.RemoveEventFromUserFavoritesListAsync("user1", 1);

            Assert.IsTrue(result);
            Assert.AreEqual(0, db.UsersEvents.Count());
        }

        // ---------- GetAllByCreatorAsync ----------
        [Test]
        public async Task GetAllByCreatorAsync_ReturnsOnlyCreatedByUser()
        {
            var db = GetDbContext(nameof(GetAllByCreatorAsync_ReturnsOnlyCreatedByUser));
            var user1 = new IdentityUser { Id = "user1", UserName = "John" };
            var user2 = new IdentityUser { Id = "user2", UserName = "Mike" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.AddRange(user1, user2);
            db.Categories.Add(category);

            db.Events.AddRange(
                     new Event { Id = 1, 
                        Name = "A", 
                        Description = "Desc A", 
                        AuthorId = "user1", 
                        CategoryId = 1, 
                        TicketPrice = 5, 
                        TotalTickets = 5 
                        },
                     new Event { Id = 2, 
                        Name = "B", 
                        Description = "Desc B", 
                        AuthorId = "user2", 
                        CategoryId = 1, 
                        TicketPrice = 5, 
                        TotalTickets = 5 
                        }
            );

            await db.SaveChangesAsync();

            var service = GetService(db);
            var result = await service.GetAllByCreatorAsync("user1");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("A", result.First().Name);
        }

        // ---------- SearchEventsAsync ----------
        [Test]
        public async Task SearchEventsAsync_FiltersByTermAndCategory()
        {
            var db = GetDbContext(nameof(SearchEventsAsync_FiltersByTermAndCategory));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category1 = new Category { Id = 1, Name = "Concert" };
            var category2 = new Category { Id = 2, Name = "Theatre" };
            db.Users.Add(user);
            db.Categories.AddRange(category1, category2);

            db.Events.AddRange(
                new Event { Id = 1, 
                    Name = "Rock Concert",
                    Description = "Test event description",
                    AuthorId = "user1",
                    CategoryId = 1,
                    TicketPrice = 10, 
                    TotalTickets = 5 
                },
                new Event { Id = 2,
                    Name = "Drama Play",
                    Description = "Test event description", 
                    AuthorId = "user1",
                    CategoryId = 2,
                    TicketPrice = 15, 
                    TotalTickets = 5 
                }
            );
            await db.SaveChangesAsync();

            var service = GetService(db);
            var result = await service.SearchEventsAsync("Rock", 1, "user1");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Rock Concert", result.First().Name);
        }

        // ---------- BuyTicketAsync ----------
        [Test]
        public async Task BuyTicketAsync_SetsTicketAsSold()
        {
            var db = GetDbContext(nameof(BuyTicketAsync_SetsTicketAsSold));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "Buy Event",
                Description = "Test description",
                TicketPrice = 10,
                TotalTickets = 5,
                AuthorId = "user1",
                CategoryId = 1
            };
            db.Events.Add(ev);

            for (int i = 0; i < 5; i++)
                db.Tickets.Add(new Ticket { EventId = 1, IsSold = false });

            await db.SaveChangesAsync();

            var service = GetService(db);
            await service.BuyTicketAsync(1, "user1", 1);

            Assert.AreEqual(1, db.Tickets.Count(t => t.IsSold));
            Assert.AreEqual(4, db.Tickets.Count(t => !t.IsSold));
        }



        // ---------- GetTicketsLeftAsync ----------
        [Test]
        public async Task GetTicketsLeftAsync_ReturnsCorrectCount()
        {
            var db = GetDbContext(nameof(GetTicketsLeftAsync_ReturnsCorrectCount));
            var user = new IdentityUser { Id = "user1", UserName = "John" };
            var category = new Category { Id = 1, Name = "Concert" };
            db.Users.Add(user);
            db.Categories.Add(category);

            var ev = new Event
            {
                Id = 1,
                Name = "Tickets Event",
                Description = "Test event description",
                TicketPrice = 10,
                TotalTickets = 3,
                AuthorId = "user1",
                CategoryId = 1
            };
            db.Events.Add(ev);

            db.Tickets.Add(new Ticket { EventId = 1, IsSold = true });
            db.Tickets.Add(new Ticket { EventId = 1, IsSold = true });
            db.Tickets.Add(new Ticket { EventId = 1, IsSold = false });

            await db.SaveChangesAsync();

            var service = GetService(db);
            var ticketsLeft = await service.GetTicketsLeftAsync(1);

            Assert.AreEqual(1, ticketsLeft, "Остават 1 непродаден билет.");
        }

    }
}
