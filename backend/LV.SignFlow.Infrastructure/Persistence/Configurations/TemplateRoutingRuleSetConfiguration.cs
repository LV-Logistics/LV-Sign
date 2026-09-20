using LV.SignFlow.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateRoutingRuleSetConfiguration : IEntityTypeConfiguration<TemplateRoutingRuleSet>
    {
        public void Configure(EntityTypeBuilder<TemplateRoutingRuleSet> builder)
        {
            builder.ToTable("TemplateRoutingRuleSets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(200);

            builder.HasOne(x=>x.TemplateVersion)
                .WithMany(x=>x.RoutingRuleSets)
                .HasForeignKey(x=>x.TemplateVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.TemplateVersionId);
        }
    }
}
