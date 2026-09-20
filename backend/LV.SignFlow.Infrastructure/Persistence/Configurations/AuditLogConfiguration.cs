using LV.SignFlow.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x=>x.Id);

            builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(x => x.EntityType)
                .HasMaxLength(200);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(1000);

            builder.HasOne(x => x.User)
                .WithMany(x => x.AuditLogs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => new
            {
                x.EntityType,
                x.EntityId
            });

            builder.HasIndex(x => x.OccurredAt);

            builder.HasIndex(x => x.Action);
        }
    }
}
