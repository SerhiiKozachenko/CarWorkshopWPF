namespace CarWorkshop.Core.Abstractions
{
    public interface IValidationError
    {
        public string Error { get; set; }
        public string PropertyName { get; set; }
        public string AttemptedValue { get; set; }
    }
}
