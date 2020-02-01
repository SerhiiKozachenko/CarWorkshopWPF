using CarWorkshop.Core.Abstractions;

namespace CarWorkshop.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Coutry { get; set; }
    }
}
