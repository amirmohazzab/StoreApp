using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class Address : BaseEntity
    {
        public string UserId { get; set; }

        public bool IsMAIN { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullAddress { get; set; }

        public string Number { get; set; }

        public string PostalCode { get; set; }

        public User User { get; set; }

        public string? Place { get; set; }
    }
}
