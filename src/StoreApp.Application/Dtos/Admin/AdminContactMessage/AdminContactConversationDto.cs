using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminContactMessage
{
    public class AdminContactConversationDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public bool HasUnRead { get; set; }

        public bool HasReply { get; set; }

        public DateTime Created { get; set; }

        public List<AdminContactMessageDto> Messages { get; set; }
    }
}
