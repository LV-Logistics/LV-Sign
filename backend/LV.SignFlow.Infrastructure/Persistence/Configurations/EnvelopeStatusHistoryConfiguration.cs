using LV.SignFlow.Domain.Entities.Envelopes;
using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class EnvelopeStatusHistoryConfiguration : IEntityTypeConfiguration<StatusHistory>
    {
        public void Configure(EntityTypeBuilder<StatusHistory> builder)
        {
            builder.ToTable("EnvelopeStatusHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Reason)
           .HasMaxLength(1000);

            builder.HasOne(x => x.Envelope)
                .WithMany(x => x.StatusHistory)
                .HasForeignKey(x => x.EnvelopeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.ChangedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => new
            {
                x.EnvelopeId,
                x.ChangedAt
            });
            builder.HasIndex(x => x.ChangedByUserId);

        }
    }
}
