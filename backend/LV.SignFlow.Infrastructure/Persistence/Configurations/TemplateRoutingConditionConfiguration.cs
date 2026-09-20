using LV.SignFlow.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateRoutingConditionConfiguration : IEntityTypeConfiguration<TemplateRoutingCondition>
    {
        public void Configure(EntityTypeBuilder<TemplateRoutingCondition> builder)
        {
            builder.ToTable("TemplateRoutingConditions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ComparisonValue)
          .HasMaxLength(1000);

            builder.HasOne(x=>x.TemplateRoutingRule)
                .WithMany(x => x.Conditions)
                .HasForeignKey(x => x.TemplateRoutingRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SourceTemplateField)
                .WithMany(x => x.TemplateRoutingConditions)
                .HasForeignKey(x => x.SourceTemplateFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TemplateRoutingRuleId);
            builder.HasIndex(x => x.SourceTemplateFieldId);

        }
    }
}
