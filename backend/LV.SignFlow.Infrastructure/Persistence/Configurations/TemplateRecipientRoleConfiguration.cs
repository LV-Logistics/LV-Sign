using LV.SignFlow.Domain.Entities.Templates;
using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateRecipientRoleConfiguration : IEntityTypeConfiguration<TemplateRecipientRole>
    {
        public void Configure(EntityTypeBuilder<TemplateRecipientRole> builder)
        {
            builder.ToTable("TemplateRecipientRoles");

            builder.HasKey(x=>x.Id);

            builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(200);

            builder.HasOne(x=>x.TemplateVersion)
                .WithMany(x=>x.RecipientsRoles)
                .HasForeignKey(x=>x.TemplateVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.DefaultUserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.TemplateVersionId,
                x.RoutingOrder
            });

            builder.HasIndex(x => x.DefaultUserId);
        }
    }
}
