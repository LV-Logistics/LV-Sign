using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class UserPermissionProfileConfiguration : IEntityTypeConfiguration<UserPermissionProfile>
    {
        public void Configure(EntityTypeBuilder<UserPermissionProfile> builder)
        {
            builder.ToTable("UserPermissionProfiles");

            builder.HasIndex(x => x.UserId);
            builder.HasKey(x => new
            {
                x.UserId,
                x.PermissionProfileId
            });

            builder.HasOne(x => x.PermissionProfile)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.PermissionProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
               .WithMany(x => x.PermissionProfiles)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
