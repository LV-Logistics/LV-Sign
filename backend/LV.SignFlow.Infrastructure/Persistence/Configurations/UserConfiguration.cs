using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x=>x.Email)
                .IsRequired()
                .HasMaxLength(320);
            builder.Property(x => x.DisplayName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.JobTitle)
                .HasMaxLength(320);
            builder.Property(x => x.ExternalIdentityId)
               .HasMaxLength(320);

            builder.HasIndex(x => x.ExternalIdentityId)
               .IsUnique()
               .HasFilter("[ExternalIdentityId] is not null");

            builder.HasOne(x=>x.Department)
                .WithMany(x=>x.Users)
                .HasForeignKey(x=>x.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Organization)
               .WithMany(x => x.Users)
               .HasForeignKey(x => x.OrganizationId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
