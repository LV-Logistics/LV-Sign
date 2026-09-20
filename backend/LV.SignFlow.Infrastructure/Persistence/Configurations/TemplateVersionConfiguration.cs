using LV.SignFlow.Domain.Entities.Templates;
using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateVersionConfiguration : IEntityTypeConfiguration<TemplateVersion>
    {
        public void Configure(EntityTypeBuilder<TemplateVersion> builder)
        {
            builder.ToTable("TemplateVersions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.VersionNumber)
           .IsRequired();

            builder.HasOne(x=>x.Template)
                .WithMany(x=>x.Versions)
                .HasForeignKey(x=>x.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(x => x.CreatedByUserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.TemplateId,
                x.VersionNumber
            })
     .IsUnique();
            builder.HasIndex(x => x.CreatedByUserId);
        }
    }
}
