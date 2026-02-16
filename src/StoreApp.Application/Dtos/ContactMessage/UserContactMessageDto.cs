using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.ContactMessage
{
    public class UserContactMessageDto
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public MessageSender Sender { get; set; }

        public DateTime Created { get; set; }

        public bool IsRead { get; set; }

        public List<ContactAttachmentDto> Attachments { get; set; } 
    }
}
