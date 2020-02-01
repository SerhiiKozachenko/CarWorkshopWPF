using CarWorkshop.Core.Abstractions;
using System;

namespace CarWorkshop.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public string Username { get; set; }
        public string CarTrademark { get; set; }
        public string CompanyName { get; set; }
        public DateTime AppointmentAt { get; set; }
    }
}
