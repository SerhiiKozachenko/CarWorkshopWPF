using CarWorkshop.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace CarWorkshop.Core.Exceptions
{
    public class ValidationErrors : Exception
    {
        public List<IValidationError> Errors { get; set; }
    }
}
