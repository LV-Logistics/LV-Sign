using LV.SignFlow.Domain.Entities.Envelopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class EnvelopeRoutingRuleConfiguration : IEntityTypeConfiguration<EnvelopeRoutingRule>
    {
        public void Configure(EntityTypeBuilder<EnvelopeRoutingRule> builder)
        {
            builder.ToTable("EnvelopeRoutingRules");
            builder.HasKey(x=>x.Id);

            builder.Property(x => x.Name)
           .HasMaxLength(200);

            builder.HasOne(x => x.EnvelopeRoutingRuleSet)
                .WithMany(x => x.Rules)
                .HasForeignKey(x => x.EnvelopeRoutingRuleSetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TargetRecipient)
                .WithMany(x => x.Rules)
                .HasForeignKey(x => x.TargetRecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.EnvelopeRoutingRuleSetId,
                x.Priority
            })
            .IsUnique();

            builder.HasIndex(x => x.TargetRecipientId);

        }
    }
}
