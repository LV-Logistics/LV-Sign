using LV.SignFlow.Domain.Authorization;
using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Seed
{
    public static class SystemSeedIds
    {
        // Roles
        public static readonly Guid AdministratorRoleId =
            Guid.Parse("10000000-0000-0000-0000-000000000001");

        public static readonly Guid StandardUserRoleId =
            Guid.Parse("10000000-0000-0000-0000-000000000002");


        // Envelope permissions
        public static readonly Guid EnvelopeView =
            Guid.Parse("20000000-0000-0000-0000-000000000001");

        public static readonly Guid EnvelopeCreate =
            Guid.Parse("20000000-0000-0000-0000-000000000002");

        public static readonly Guid EnvelopeSend =
            Guid.Parse("20000000-0000-0000-0000-000000000003");

        public static readonly Guid EnvelopeVoid =
            Guid.Parse("20000000-0000-0000-0000-000000000004");

        public static readonly Guid EnvelopeDelete =
            Guid.Parse("20000000-0000-0000-0000-000000000005");


        // Template permissions
        public static readonly Guid TemplateView =
            Guid.Parse("20000000-0000-0000-0000-000000000006");

        public static readonly Guid TemplateCreate =
            Guid.Parse("20000000-0000-0000-0000-000000000007");

        public static readonly Guid TemplateEdit =
            Guid.Parse("20000000-0000-0000-0000-000000000008");

        public static readonly Guid TemplateDelete =
            Guid.Parse("20000000-0000-0000-0000-000000000009");

        public static readonly Guid TemplateUse =
            Guid.Parse("20000000-0000-0000-0000-000000000010");

        public static readonly Guid TemplateShare =
            Guid.Parse("20000000-0000-0000-0000-000000000011");


        // User permissions
        public static readonly Guid UserView =
            Guid.Parse("20000000-0000-0000-0000-000000000012");

        public static readonly Guid UserManage =
            Guid.Parse("20000000-0000-0000-0000-000000000013");


        // Role permissions
        public static readonly Guid RoleView =
            Guid.Parse("20000000-0000-0000-0000-000000000014");

        public static readonly Guid RoleManage =
            Guid.Parse("20000000-0000-0000-0000-000000000015");


        // Permission Profile permissions
        public static readonly Guid PermissionProfileView =
            Guid.Parse("20000000-0000-0000-0000-000000000016");

        public static readonly Guid PermissionProfileManage =
            Guid.Parse("20000000-0000-0000-0000-000000000017");


        // Reports
        public static readonly Guid ReportView =
            Guid.Parse("20000000-0000-0000-0000-000000000018");


        // Audit
        public static readonly Guid AuditView =
            Guid.Parse("20000000-0000-0000-0000-000000000019");


        // Stamps
        public static readonly Guid StampView =
            Guid.Parse("20000000-0000-0000-0000-000000000020");

        public static readonly Guid StampManage =
            Guid.Parse("20000000-0000-0000-0000-000000000021");
    }


    public static class SystemSeedData
    {
        private static readonly DateTimeOffset SeedDate =
            new(2026, 9, 21, 0, 0, 0, TimeSpan.Zero);

        public static readonly Role[] Roles =
        {
        new()
        {
            Id = SystemSeedIds.AdministratorRoleId,
            Name = SystemRoleNames.Administrator,
            Description = "System administrator with full system access.",
            IsSystemRole = true,
            IsActive = true,
            CreatedAt = SeedDate,
            UpdatedAt = SeedDate
        },

        new()
        {
            Id = SystemSeedIds.StandardUserRoleId,
            Name = SystemRoleNames.StandardUser,
            Description = "Standard internal LV-Sign user.",
            IsSystemRole = true,
            IsActive = true,
            CreatedAt = SeedDate,
            UpdatedAt = SeedDate
        }
    };


        public static readonly Permission[] Permissions =
        {
        new()
        {
            Id = SystemSeedIds.EnvelopeView,
            Code = PermissionCodes.Envelopes.View,
            Name = "View Envelopes",
            Category = "Envelopes"
        },
        new()
        {
            Id = SystemSeedIds.EnvelopeCreate,
            Code = PermissionCodes.Envelopes.Create,
            Name = "Create Envelopes",
            Category = "Envelopes"
        },
        new()
        {
            Id = SystemSeedIds.EnvelopeSend,
            Code = PermissionCodes.Envelopes.Send,
            Name = "Send Envelopes",
            Category = "Envelopes"
        },
        new()
        {
            Id = SystemSeedIds.EnvelopeVoid,
            Code = PermissionCodes.Envelopes.Void,
            Name = "Void Envelopes",
            Category = "Envelopes"
        },
        new()
        {
            Id = SystemSeedIds.EnvelopeDelete,
            Code = PermissionCodes.Envelopes.Delete,
            Name = "Delete Envelopes",
            Category = "Envelopes"
        },

        new()
        {
            Id = SystemSeedIds.TemplateView,
            Code = PermissionCodes.Templates.View,
            Name = "View Templates",
            Category = "Templates"
        },
        new()
        {
            Id = SystemSeedIds.TemplateCreate,
            Code = PermissionCodes.Templates.Create,
            Name = "Create Templates",
            Category = "Templates"
        },
        new()
        {
            Id = SystemSeedIds.TemplateEdit,
            Code = PermissionCodes.Templates.Edit,
            Name = "Edit Templates",
            Category = "Templates"
        },
        new()
        {
            Id = SystemSeedIds.TemplateDelete,
            Code = PermissionCodes.Templates.Delete,
            Name = "Delete Templates",
            Category = "Templates"
        },
        new()
        {
            Id = SystemSeedIds.TemplateUse,
            Code = PermissionCodes.Templates.Use,
            Name = "Use Templates",
            Category = "Templates"
        },
        new()
        {
            Id = SystemSeedIds.TemplateShare,
            Code = PermissionCodes.Templates.Share,
            Name = "Share Templates",
            Category = "Templates"
        },

        new()
        {
            Id = SystemSeedIds.UserView,
            Code = PermissionCodes.Users.View,
            Name = "View Users",
            Category = "Users"
        },
        new()
        {
            Id = SystemSeedIds.UserManage,
            Code = PermissionCodes.Users.Manage,
            Name = "Manage Users",
            Category = "Users"
        },

        new()
        {
            Id = SystemSeedIds.RoleView,
            Code = PermissionCodes.Roles.View,
            Name = "View Roles",
            Category = "Roles"
        },
        new()
        {
            Id = SystemSeedIds.RoleManage,
            Code = PermissionCodes.Roles.Manage,
            Name = "Manage Roles",
            Category = "Roles"
        },

        new()
        {
            Id = SystemSeedIds.PermissionProfileView,
            Code = PermissionCodes.PermissionProfiles.View,
            Name = "View Permission Profiles",
            Category = "Permission Profiles"
        },
        new()
        {
            Id = SystemSeedIds.PermissionProfileManage,
            Code = PermissionCodes.PermissionProfiles.Manage,
            Name = "Manage Permission Profiles",
            Category = "Permission Profiles"
        },

        new()
        {
            Id = SystemSeedIds.ReportView,
            Code = PermissionCodes.Reports.View,
            Name = "View Reports",
            Category = "Reports"
        },

        new()
        {
            Id = SystemSeedIds.AuditView,
            Code = PermissionCodes.Audit.View,
            Name = "View Audit Logs",
            Category = "Audit"
        },

        new()
        {
            Id = SystemSeedIds.StampView,
            Code = PermissionCodes.Stamps.View,
            Name = "View Stamps",
            Category = "Stamps"
        },
        new()
        {
            Id = SystemSeedIds.StampManage,
            Code = PermissionCodes.Stamps.Manage,
            Name = "Manage Stamps",
            Category = "Stamps"
        }
    };
    }
}
