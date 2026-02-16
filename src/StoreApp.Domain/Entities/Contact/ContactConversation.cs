using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Contact
{
    public class ContactConversation : BaseAuditableEntity
    {
        public string Name { get; set; }      
        
        public string Email { get; set; }         

        public string Subject { get; set; }

        public string? UserId { get; set; }    

        public User.User? User { get; set; }

        public bool IsClosed { get; set; } = false;

        public bool HasAdminReply { get; set; } = false;

        public ICollection<ContactMessage> Messages { get; set; } = new List<ContactMessage>();
    }
}
