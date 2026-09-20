using LV.SignFlow.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateDocumentConfiguration : IEntityTypeConfiguration<TemplateDocument>
    {
        public void Configure(EntityTypeBuilder<TemplateDocument> builder)
        {
            builder.ToTable("TemplateDocuments");

            builder.HasKey(t => t.Id);

            builder.Property(x => x.OriginalFileName)
           .IsRequired()
           .HasMaxLength(500);

            builder.Property(x => x.BlobPath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.FileHash)
                .HasMaxLength(128);

            builder.HasOne(x=>x.TemplateVersion)
                .WithMany(x=>x.Documents)
                .HasForeignKey(x=>x.TemplateVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.TemplateVersionId,
                x.Order
            });

        }
    }
}
