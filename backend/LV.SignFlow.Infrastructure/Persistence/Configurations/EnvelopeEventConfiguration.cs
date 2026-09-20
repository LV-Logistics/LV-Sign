using LV.SignFlow.Domain.Entities.Envelopes;
using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class EnvelopeEventConfiguration : IEntityTypeConfiguration<Events>
    {
        public void Configure(EntityTypeBuilder<Events> builder)
        {
            builder.ToTable("EvelopeEvents");

            builder.HasKey(e => e.Id);

            builder.Property(x => x.IpAddress)
           .HasMaxLength(45);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(1000);

            builder.HasOne(e=>e.Envelope)
                .WithMany(e => e.Events)
                .HasForeignKey(e=>e.EnvelopeId)
                .OnDelete(DeleteBehavior.Cascade);
            //
            builder.HasOne(e => e.Recipient)
               .WithMany(e => e.Events)
               .HasForeignKey(e => e.RecipientId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => new
            {
                x.EnvelopeId,
                x.OccurredAt
            });

            builder.HasIndex(x => x.RecipientId);

            builder.HasIndex(x => x.UserId);

        }
    }
}
