using StoreApp.Domain.Entities.Base;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Contact
{
    public class ContactMessage : BaseAuditableEntity
    {
        public int ConversationId { get; set; }

        public ContactConversation Conversation { get; set; }

        public string Message { get; set; }

        public MessageSender Sender { get; set; }

        public bool IsRead { get; set; } = false;

        public ICollection<ContactAttachment> Attachments { get; set; } = new List<ContactAttachment>();

    }
}
