using LV.SignFlow.Domain.Entities.Envelopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class EnvelopeRoutingConditionConfiguration : IEntityTypeConfiguration<EnvelopeRoutingCondition>
    {
        public void Configure(EntityTypeBuilder<EnvelopeRoutingCondition> builder)
        {
            builder.ToTable("EnvelopeRoutingConditions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ComparisonValue)
            .HasMaxLength(1000);

            builder.HasOne(x => x.EnvelopeRoutingRule)
                .WithMany(x => x.Conditions)
                .HasForeignKey(x => x.EnvelopeRoutingRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SourceEnvelopeField)
                .WithMany(x => x.EnvelopeRoutingConditions)
                .HasForeignKey(x => x.SourceEnvelopeFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.EnvelopeRoutingRuleId);

            builder.HasIndex(x => x.SourceEnvelopeFieldId);

        }
    }
}
