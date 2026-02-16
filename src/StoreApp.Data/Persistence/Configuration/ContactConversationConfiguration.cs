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
    public class ContactConversationConfiguration : IEntityTypeConfiguration<ContactConversation>
    {
        public void Configure(EntityTypeBuilder<ContactConversation> builder)
        {
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Subject).HasMaxLength(200).IsRequired();

            builder.HasIndex(x => new { x.Email, x.Subject });

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
