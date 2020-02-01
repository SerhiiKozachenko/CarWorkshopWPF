namespace CarWorkshop.Core.Abstractions
{
    public class BaseEntity : IHasID<long>
    {
        public long Id { get; set; }
    }
}
