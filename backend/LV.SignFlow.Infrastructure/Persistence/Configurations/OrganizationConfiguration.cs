using LV.SignFlow.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x=>x.Code) .HasMaxLength(50);
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[Code] is not null");
        }
    }
}
