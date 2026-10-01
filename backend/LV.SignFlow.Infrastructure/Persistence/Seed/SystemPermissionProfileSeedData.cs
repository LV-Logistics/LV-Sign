using LV.SignFlow.Domain.Authorization;
using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Seed
{

    public static class SystemPermissionProfileSeedIds
    {
        public static readonly Guid StandardSenderProfileId =
            Guid.Parse("30000000-0000-0000-0000-000000000001");

        public static readonly Guid TemplateManagerProfileId =
            Guid.Parse("30000000-0000-0000-0000-000000000002");
    }

    public static class SystemPermissionProfileSeedData
    {
        private static readonly DateTimeOffset SeedDate =
            new(2026, 9, 21, 0, 0, 0, TimeSpan.Zero);

        public static readonly PermissionProfile[] Profiles =
        {
        new()
        {
            Id = SystemPermissionProfileSeedIds.StandardSenderProfileId,
            Name = SystemPermissionProfileNames.StandardSender,
            Description = "Allows a user to create and send envelopes and use existing templates.",
            IsSystemProfile = true,
            IsActive = true,
            CreatedAt = SeedDate,
            UpdatedAt = SeedDate
        },

        new()
        {
            Id = SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            Name = SystemPermissionProfileNames.TemplateManager,
            Description = "Allows a user to create, edit, use and share templates.",
            IsSystemProfile = true,
            IsActive = true,
            CreatedAt = SeedDate,
            UpdatedAt = SeedDate
        }
    };

        public static readonly PermissionProfilePermission[] ProfilePermissions =
        {
        // Standard Sender
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.StandardSenderProfileId,
            PermissionId = SystemSeedIds.EnvelopeView
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.StandardSenderProfileId,
            PermissionId = SystemSeedIds.EnvelopeCreate
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.StandardSenderProfileId,
            PermissionId = SystemSeedIds.EnvelopeSend
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.StandardSenderProfileId,
            PermissionId = SystemSeedIds.TemplateView
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.StandardSenderProfileId,
            PermissionId = SystemSeedIds.TemplateUse
        },

        // Template Manager
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            PermissionId = SystemSeedIds.TemplateView
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            PermissionId = SystemSeedIds.TemplateCreate
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            PermissionId = SystemSeedIds.TemplateEdit
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            PermissionId = SystemSeedIds.TemplateDelete
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            PermissionId = SystemSeedIds.TemplateUse
        },
        new()
        {
            PermissionProfileId =
                SystemPermissionProfileSeedIds.TemplateManagerProfileId,
            PermissionId = SystemSeedIds.TemplateShare
        }
    };

    }
}
