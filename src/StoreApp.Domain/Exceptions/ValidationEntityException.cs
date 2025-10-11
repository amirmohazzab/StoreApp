using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace StoreApp.Domain.Exceptions
{
    public class ValidationEntityException : BaseException
    {
        public ValidationEntityException(List<string> messages) : base(messages)
        {
        }

        public ValidationEntityException(string message) : base(message)
        {
        }

        public ValidationEntityException() : base("Error happened, please try again")
        {

        }

        public ValidationEntityException(IEnumerable<ValidationFailure> validationFailures) : base(validationFailures)
        {

        }
    }
}
