using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Contact
{
    public class ContactAttachment : BaseEntity
    {
        public int ContactMessageId { get; set; }

        public ContactMessage ContactMessage { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}
