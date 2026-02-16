using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.ContactMessage
{
    public class UserContactConversationDto
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public DateTime Created { get; set; }

        public bool HasUnreadByAdmin { get; set; } 

        public bool HasReply { get; set; }
    }
}
