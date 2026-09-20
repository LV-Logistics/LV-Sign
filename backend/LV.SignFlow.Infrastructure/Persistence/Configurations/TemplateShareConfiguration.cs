using LV.SignFlow.Domain.Entities.Organizations;
using LV.SignFlow.Domain.Entities.Templates;
using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Configurations
{
    public class TemplateShareConfiguration : IEntityTypeConfiguration<TemplateShare>
    {
        public void Configure(EntityTypeBuilder<TemplateShare> builder)
        {
            builder.ToTable("TemplateShares");

            builder.HasKey(t => t.Id);

            builder.HasOne(t=>t.Template)
                .WithMany(t=>t.Shares)
                .HasForeignKey(t=>t.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(x => x.SharedWithDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.SharedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TemplateId);

            builder.HasIndex(x => x.SharedWithUserId);

            builder.HasIndex(x => x.SharedWithDepartmentId);

            builder.HasIndex(x => x.SharedByUserId);

            builder.ToTable(
                "TemplateShares",
                table => table.HasCheckConstraint(
                    "CK_TemplateShares_ExactlyOneTarget",
                    @"(
                    ([SharedWithUserId] IS NOT NULL AND [SharedWithDepartmentId] IS NULL)
                    OR
                    ([SharedWithUserId] IS NULL AND [SharedWithDepartmentId] IS NOT NULL)
                  )"));
        }
    }
}
