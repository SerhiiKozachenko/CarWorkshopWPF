using CarWorkshop.Core.Abstractions;

namespace CarWorkshop.Core.Entities
{
    public class Workshop : BaseEntity
    {
        public string CompanyName { get; set; }
        public string CarTrademarks { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
