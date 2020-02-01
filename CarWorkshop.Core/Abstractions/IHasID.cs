namespace CarWorkshop.Core.Abstractions
{
    public interface IHasID<T> where T : struct
    {
        public T Id { get; set; }
    }
}
