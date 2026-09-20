using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class PermissionProfilePermissionConfiguration : IEntityTypeConfiguration<PermissionProfilePermission>
    {
        public void Configure(EntityTypeBuilder<PermissionProfilePermission> builder)
        {
            builder.ToTable("PermissionProfilePermissions");

            builder.HasKey(x => new
            {
                x.PermissionProfileId,
                x.PermissionId
            });

            builder.HasOne(x => x.PermissionProfile)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.PermissionProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.PermissionProfilePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
