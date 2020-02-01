using CarWorkshop.Core.Abstractions;
using System;

namespace CarWorkshop.Core.Exceptions
{
    public class ValidationError : Exception, IValidationError
    {
        public string Error { get; set; }
        public string PropertyName { get; set; }
        public string AttemptedValue { get; set; }

        public ValidationError(string error, string propertyName, string attemptedValue)
        {
            this.Error = error;
            this.PropertyName = propertyName;
            this.AttemptedValue = attemptedValue;
        }
    }
}
