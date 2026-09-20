using LV.SignFlow.Domain.Entities.Envelopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class EnvelopeRoutingRuleSetConfiguration : IEntityTypeConfiguration<EnvelopeRoutingRuleSet>
    {
        public void Configure(EntityTypeBuilder<EnvelopeRoutingRuleSet> builder)
        {
            builder.ToTable("EnvelopeRoutingRuleSets");
            builder.HasKey(e => e.Id);

            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

            builder.HasOne(x => x.Envelope)
                .WithMany(x => x.EnvelopeRoutingRuleSets)
                .HasForeignKey(x => x.EnvelopeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.EnvelopeId);

        }
    }
}
