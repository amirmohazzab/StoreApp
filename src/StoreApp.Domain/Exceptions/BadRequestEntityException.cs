using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Exceptions
{
    public class BadRequestEntityException : BaseException
    {
        public BadRequestEntityException(List<string> messages) : base(messages)
        {
        }

        public BadRequestEntityException(string message) : base(message)
        {
        }

        public BadRequestEntityException() : base("Error happened, please try again")
        {

        }
    }
}
