using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminContactMessage
{
    public class AdminContactMessageDto
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public MessageSender Sender { get; set; }

        public bool IsRead { get; set; }

        public DateTime Created { get; set; }

        public List<AdminContactAttachmentDto> Attachments { get; set; }  
    }
}
