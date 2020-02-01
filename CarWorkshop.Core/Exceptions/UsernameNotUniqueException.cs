using CarWorkshop.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarWorkshop.Core.Exceptions
{
    public class UsernameNotUniqueException : IValidationError
    {
        public string Error { get; set; }
        public string PropertyName { get; set; }
        public string AttemptedValue { get; set; }
    }
}
