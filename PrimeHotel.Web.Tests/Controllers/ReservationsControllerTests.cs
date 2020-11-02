using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PrimeHotel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PrimeHotel.Web.Controllers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace PrimeHotel.Web.Tests
{
    public class ReservationsControllerTests
    {
        private DbContextOptions<PrimeDbContext> dbContextOptions = new DbContextOptionsBuilder<PrimeDbContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;
        private ReservationsController controller;

        [OneTimeSetUp]
        public void Setup()
        {
            SeedDb();

            controller = new ReservationsController(new PrimeDbContext(dbContextOptions));
        }

        [Test]
        public async Task Get_FetchesReservationsWithoutRoomsAndGuests()
        {
            using var context = new PrimeDbContext(dbContextOptions);
            var reservations = (await controller.Get()).ToList();

            reservations.Count.Should().Be(2);
            reservations.All(r => r.Room == null).Should().BeFalse();
            reservations.All(r => r.Profiles == null).Should().BeTrue();
        }

        [Test]
        public async Task GetById_WhenIdIsProvided_FetchesReservationWithRoomsAndGuests()
        {
            using var context = new PrimeDbContext(dbContextOptions);
            var result = await controller.GetById(2);
            var okResult = result.As<OkObjectResult>();
            var reservation = okResult.Value.As<Reservation>();

            reservation.Should().NotBeNull();
            reservation.Profiles.Should().NotBeNull();
            reservation.Room.Should().NotBeNull();
        }

        [Test]
        public async Task Post_WithRoomAndProfiles_AddsReservation()
        {
            var newReservation = new NewReservation
            {
                From = DateTime.Today.AddDays(3),
                To = DateTime.Today.AddDays(7),
                RoomId = 3,
                GuestIds = new List<int> { 2 }
            };

            using var context = new PrimeDbContext(dbContextOptions);
            var result = await controller.Post(newReservation);

            var okResult = result.As<OkObjectResult>();
            var reservationId = okResult.Value.As<int>();
            var addedReservation = await context.Reservations
                .Include(p => p.Profiles)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            addedReservation.Should().NotBeNull();
            addedReservation.Profiles.Should().NotBeNull();
            addedReservation.Profiles.Count.Should().Be(1);
            addedReservation.Profiles[0].Id.Should().Be(2);
            addedReservation.Room.Should().NotBeNull();
            addedReservation.Room.Id.Should().Be(3);
        }

        private void SeedDb()
        {
            using var context = new PrimeDbContext(dbContextOptions);
            var rooms = new List<Room>
            {
                new Room { Id = 1, Description = "Room nr 1", Number = 1, Level = 1, RoomType = RoomType.Standard },
                new Room { Id = 2, Description = "Room nr 2", Number = 2, Level = 1, RoomType = RoomType.Standard },
                new Room { Id = 3, Description = "Room nr 3", Number = 3, Level = 2, RoomType = RoomType.Suite }
            };

            var profiles = new List<Profile>
            {
                new Profile { Id = 1, Ref = "Profile 1", Forename = "Michał", Surname = "Białecki" },
                new Profile { Id = 2, Ref = "Profile 2", Forename = "John", Surname = "Show" },
                new Profile { Id = 3, Ref = "Profile 3", Forename = "Daenerys", Surname = "Targaryen" }
            };

            context.AddRange(rooms);
            context.AddRange(profiles);

            context.AddRange(new List<Reservation>
            {
                new Reservation
                { 
                    Id = 1,
                    Room = rooms[0],
                    Profiles = new List<Profile>{ profiles[0] },
                    From = DateTime.Today,
                    To = DateTime.Today.AddDays(2)
                },
                new Reservation
                {
                    Id = 2,
                    Room = rooms[2],
                    Profiles = new List<Profile>{ profiles[1], profiles[2] },
                    From = DateTime.Today.AddDays(1),
                    To = DateTime.Today.AddDays(3)
                }
            });

            context.SaveChanges();
        }
    }
}