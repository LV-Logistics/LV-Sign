using LV.SignFlow.Domain.Entities.Signing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class StampConfiguration : IEntityTypeConfiguration<Stamps>
    {
        public void Configure(EntityTypeBuilder<Stamps> builder)
        {
            builder.ToTable("Stamps");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(200);

            builder.Property(x => x.BlobPath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.Stamps)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.CreatedByUserId);

            builder.HasIndex(x => x.Name);
        }
    }
}
