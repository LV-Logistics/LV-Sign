using LV.SignFlow.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Code)
                .HasMaxLength(50);
            builder.HasIndex(x => new
            {
                x.OrganizationId,
                x.Name
            })
   .IsUnique();

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.Departments)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
