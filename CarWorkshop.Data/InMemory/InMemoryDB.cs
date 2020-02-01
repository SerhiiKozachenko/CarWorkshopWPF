using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Data.InMemory
{
    public class InMemoryDB : IUnitOfWork
    {
        private readonly IList<object> _users;
        private readonly IList<object> _workshops;
        private readonly IList<object> _appointments;

        private readonly ConcurrentDictionary<Type, IList<object>> _db;

        public InMemoryDB()
        {
            _users = new List<object>();
            _workshops = new List<object>();
            _appointments = new List<object>();
            _db = new ConcurrentDictionary<Type, IList<object>>()
            {
                [typeof(User)] = _users,
                [typeof(Workshop)] = _workshops,
                [typeof(Appointment)] = _appointments,
            };
            _FillInDefaultData();
        }

        private void _FillInDefaultData()
        {
            _users.Add(new User() { Id = 1, Username = "John", Email = "john@mail.com", City = "Berlin", Coutry = "Germany", PostalCode = "10115" });
            _users.Add(new User() { Id = 2, Username = "Stefani", Email = "stefani@mail.com", City = "Berlin", Coutry = "Germany", PostalCode = "10178" });
            _users.Add(new User() { Id = 3, Username = "David", Email = "david@mail.com", City = "Munich", Coutry = "Germany", PostalCode = "80331" });
            _users.Add(new User() { Id = 4, Username = "Joana", Email = "joana@mail.com", City = "Munich", Coutry = "Germany", PostalCode = "80337" });

            _workshops.Add(new Workshop() { Id = 1, CompanyName = "Auto Service Munich", CarTrademarks = "BMW, VW, Mercedes, Ford", City = "Munich", Country = "Germany", PostalCode = "80331" });
            _workshops.Add(new Workshop() { Id = 2, CompanyName = "Auto Service Light", CarTrademarks = "VW, Ford", City = "Munich", Country = "Germany", PostalCode = "80337" });
            _workshops.Add(new Workshop() { Id = 3, CompanyName = "Lux Auto Service", CarTrademarks = "BMW, Audi, Mercedes", City = "Berlin", Country = "Germany", PostalCode = "10115" });
            _workshops.Add(new Workshop() { Id = 4, CompanyName = "Best Auto Service", CarTrademarks = "VW, Audi, Ford", City = "Berlin", Country = "Germany", PostalCode = "10178" });

            _appointments.Add(new Appointment() { Id = 1, Username = "John", CarTrademark = "BMW", CompanyName = "Lux Auto Service", AppointmentAt = DateTime.Now.AddDays(1) });
            _appointments.Add(new Appointment() { Id = 2, Username = "David", CarTrademark = "Ford", CompanyName = "Auto Service Munich", AppointmentAt = DateTime.Now.AddDays(2) });
        }

        public IList<T> List<T>() where T : BaseEntity
        {
            return _db[typeof(T)].OfType<T>().ToList();
        }

        public void Add<T>(T entity) where T : BaseEntity
        {
            _db[typeof(T)].Add(entity);
        }

        public void Remove<T>(T entity) where T : BaseEntity
        {
            _db[typeof(T)].Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            // no transaction
        }
    }
}
