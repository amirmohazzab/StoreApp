using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    public class ContactAttachmentConfiguration : IEntityTypeConfiguration<ContactAttachment>
    {
        public void Configure(EntityTypeBuilder<ContactAttachment> builder)
        {
            builder.HasOne(x => x.ContactMessage)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.ContactMessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
