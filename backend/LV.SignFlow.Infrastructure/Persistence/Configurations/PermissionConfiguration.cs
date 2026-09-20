using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
             .IsRequired()
             .HasMaxLength(150);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Category)
                .HasMaxLength(100);

            builder.HasIndex(x => x.Code)
                .IsUnique();

            
        }
    }
}
