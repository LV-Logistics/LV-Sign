using LV.SignFlow.Domain.Entities.Organizations;
using LV.SignFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Seed
{
    public static class DevelopmentDataSeeder
    {
        public static readonly Guid DevelopmentUserId =
       Guid.Parse("40000000-0000-0000-0000-000000000001");

        public static readonly Guid DevelopmentOrganizationId =
            Guid.Parse("40000000-0000-0000-0000-000000000002");

        public static async Task SeedAsync(
            AppDbContext context,
            CancellationToken cancellationToken = default)
        {
            var organizationExists =
                await context.Organizations.AnyAsync(
                    x => x.Id == DevelopmentOrganizationId,
                    cancellationToken);

            if (!organizationExists)
            {
                context.Organizations.Add(new Organization
                {
                    Id = DevelopmentOrganizationId,
                    Name = "LV Logistics - Development",
                    Code = "LV-DEV",
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }

            var userExists =
                await context.Users.AnyAsync(
                    x => x.Id == DevelopmentUserId,
                    cancellationToken);

            if (!userExists)
            {
                context.Users.Add(new User
                {
                    Id = DevelopmentUserId,
                    OrganizationId = DevelopmentOrganizationId,
                    Email = "example@lv-logistics.com",
                    DisplayName = "Development User",
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
