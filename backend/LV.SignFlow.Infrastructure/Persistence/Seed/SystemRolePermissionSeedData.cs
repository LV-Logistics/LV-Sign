using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Infrastructure.Persistence.Seed
{
    public static class SystemRolePermissionSeedData
    {
        public static readonly RolePermission[] RolePermissions =
        {
        // ============================================
        // Administrator - Envelope
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.EnvelopeView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.EnvelopeCreate
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.EnvelopeSend
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.EnvelopeVoid
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.EnvelopeDelete
        },

        // ============================================
        // Administrator - Templates
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.TemplateView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.TemplateCreate
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.TemplateEdit
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.TemplateDelete
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.TemplateUse
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.TemplateShare
        },

        // ============================================
        // Administrator - Users
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.UserView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.UserManage
        },

        // ============================================
        // Administrator - Roles
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.RoleView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.RoleManage
        },

        // ============================================
        // Administrator - Permission Profiles
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.PermissionProfileView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.PermissionProfileManage
        },

        // ============================================
        // Administrator - Reports / Audit / Stamps
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.ReportView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.AuditView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.StampView
        },
        new()
        {
            RoleId = SystemSeedIds.AdministratorRoleId,
            PermissionId = SystemSeedIds.StampManage
        },

        // ============================================
        // Standard User - basic access only
        // ============================================

        new()
        {
            RoleId = SystemSeedIds.StandardUserRoleId,
            PermissionId = SystemSeedIds.EnvelopeView
        },
        new()
        {
            RoleId = SystemSeedIds.StandardUserRoleId,
            PermissionId = SystemSeedIds.TemplateView
        }
    };
    }
}
