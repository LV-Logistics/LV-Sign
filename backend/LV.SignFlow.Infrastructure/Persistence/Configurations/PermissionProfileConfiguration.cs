using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class PermissionProfileConfiguration : IEntityTypeConfiguration<PermissionProfile>
    {
        public void Configure(EntityTypeBuilder<PermissionProfile> builder)
        {
            builder.ToTable("PermissionProfiles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
          .IsRequired()
          .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasIndex(x => x.Name)
                .IsUnique();

        }
    }
}
