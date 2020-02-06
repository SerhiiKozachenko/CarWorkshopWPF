using CarWorkshop.Core.Services;
using NUnit.Framework;
using Moq;
using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using CarWorkshop.Core.Exceptions;

namespace CarWorkshop.Core.Tests
{
    public class WorkshopServiceTests
    {
        private Mock<IRepositoryAsync<User>> _usersRepoMock;
        private Mock<IRepositoryAsync<Workshop>> _workshopsRepoMock;
        private Mock<IRepositoryAsync<Appointment>> _appointmentsRepoMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private AppointmentServiceAsync _Subj
        {
            get => new AppointmentServiceAsync(_appointmentsRepoMock.Object, _usersRepoMock.Object, _workshopsRepoMock.Object, _unitOfWorkMock.Object);
        }

        public WorkshopServiceTests()
        {
            _usersRepoMock = new Mock<IRepositoryAsync<User>>();
            _workshopsRepoMock = new Mock<IRepositoryAsync<Workshop>>();
            _appointmentsRepoMock = new Mock<IRepositoryAsync<Appointment>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [SetUp]
        public void Setup()
        {
            // Fill in some default data here
        }

        [Test]
        public async Task Can_validate_car_trademark()
        {
            var appointment = new Appointment()
            {
                Username = "test_user",
                CarTrademark = "bmw X6",
                CompanyName = "AutoService",
                AppointmentAt = DateTime.Today.AddHours(12)
            };

            var users = new List<User>()
            {
                new User()
                {
                    Username = appointment.Username,
                    City = "Berlin",
                    Country = "Germany",
                    PostalCode = "10123",
                    Email = "test@mail.com"
                }
            }.AsQueryable();
            _usersRepoMock.Setup(m => m.QueryAsync()).ReturnsAsync(users);

            var workshops = new List<Workshop>()
            {
                new Workshop()
                {
                    CompanyName = appointment.CompanyName,
                    CarTrademarks = "not" + appointment.CarTrademark,
                    City = "Berlin",
                    Country = "Germany",
                    PostalCode = "10123"
                }
            }.AsQueryable();
            _workshopsRepoMock.Setup(m => m.QueryAsync()).ReturnsAsync(workshops);

            
            try
            {
                Assert.ThrowsAsync<ValidationErrors>(async () => await _Subj.AddAsync(appointment));
            }
            catch (ValidationErrors ex)
            {
                Assert.That(ex.Errors.First().Error, Is.EqualTo("Car trademark is not supported"));
            }
        }
    }
}