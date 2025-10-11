using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Exceptions
{
    public class BaseException : Exception
    {
        public List<string> Messages { get; set; }

        public BaseException(List<string> messages) : base(null)
        {
            Messages = messages;
        }

        public BaseException(string message) : base(message)
        {
            
        }

        public BaseException(IEnumerable<ValidationFailure> validationFailures)
        {
            Messages = validationFailures.Select(x => x.ErrorMessage).ToList();
        }
    }
}
