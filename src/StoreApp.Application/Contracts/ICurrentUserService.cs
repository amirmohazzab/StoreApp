using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Contracts
{
    public interface ICurrentUserService
    {
        public string UserId { get; }

        public string PhoneNumber { get; }
    }
}
