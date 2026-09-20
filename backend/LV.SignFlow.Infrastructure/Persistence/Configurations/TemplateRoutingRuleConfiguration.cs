using LV.SignFlow.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateRoutingRuleConfiguration : IEntityTypeConfiguration<TemplateRoutingRule>
    {
        public void Configure(EntityTypeBuilder<TemplateRoutingRule> builder)
        {
            builder.ToTable("TemplateRoutingRules");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
            .HasMaxLength(200);

            builder.HasOne(x => x.TemplateRoutingRuleSet)
    .WithMany(x => x.Rules)
    .HasForeignKey(x => x.TemplateRoutingRuleSetId)
    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TargetRecipientRole)
    .WithMany(x => x.Rules)
    .HasForeignKey(x => x.TargetRecipientRoleId)
    .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TargetRecipientRoleId);

            builder.HasIndex(x => new
            {
                x.TemplateRoutingRuleSetId,
                x.Priority
            })
    .IsUnique();
        }
    }
}
