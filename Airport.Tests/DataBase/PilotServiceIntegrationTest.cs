﻿using NUnit.Framework;
using FluentValidation;
using AutoMapper;
using Airport.BLL.Mappers;
using Airport.DAL.Entities;
using Airport.BLL.Validators;
using Airport.DAL.Interfaces;
using Airport.Shared.DTO;
using Airport.BLL.Services;
using Airport.DAL;
using Airport.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Airport.Tests.IntegrationTests
{
    [TestFixture]
    public class PilotServiceIntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        private IValidator<Pilot> validator;
        private IPilotService service;
        private AirportContext db;
        private Pilot testItem;


        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new GeneralMapperProfile()));
            mapper = config.CreateMapper();

            var builder = new DbContextOptionsBuilder<AirportContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AirportDb(Bilyk);Trusted_Connection=True;");

            db = new AirportContext(builder.Options);
            db.Database.BeginTransaction();

            unitOfWork = new UnitOfWork(db);
            validator = new PilotValidator();
            service = new PilotService(unitOfWork, mapper, validator);

            // Adding test item to db for updating and getting and deleting in tests 
            testItem = Initializer.PilotFaker.Generate();
            db.Pilots.Add(testItem);
            db.SaveChanges();
        }


        [Test]
        public void Get_WhenGetElementByKnownId_ThenReturnedNotNullObject()
        {
            // Arrange

            // Act
            var returnedCount = service.Get(testItem.Id);

            // Assert
            Assert.IsNotNull(returnedCount);
        }

        [Test]
        public void GetAll_WhenGetAllElements_ThenReturnedCountIsTheSameLikeInDb()
        {
            // Arrange
            var amountBeforeCreating = db.Pilots.Count();

            // Act
            var returnedCount = service.GetAll().Count;

            // Assert
            Assert.AreEqual(returnedCount, amountBeforeCreating);
        }

        [Test]
        public void Create_WhenCreatedNewItem_ThenElementsInDbBecomePlusOne()
        {
            // Arrange
            var item = Initializer.PilotFaker.Generate();
            var dto = mapper.Map<Pilot, PilotDto>(item);
            var amountBeforeCreating = db.Pilots.Count();

            // Act
            service.Create(dto);

            // Assert
            Assert.AreEqual(amountBeforeCreating + 1, db.Pilots.Count());
        }

        [Test]
        public void Delete_WhendeletingItem_ThenElementsInDbBecomeMinusOne()
        {
            // Arrange
            var amountBeforeDeleting = db.Pilots.Count();

            // Act
            service.Delete(testItem.Id);

            // Assert
            Assert.AreEqual(amountBeforeDeleting - 1, db.Pilots.Count());
        }

        [TearDown]
        public void Cleanup()
        {
            db.Database.RollbackTransaction();
        }
    }
}
