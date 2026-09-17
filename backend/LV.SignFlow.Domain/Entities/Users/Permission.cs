using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Users
{
    public class Permission
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Category { get; set; }


        public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();

        public ICollection<PermissionProfilePermission> PermissionProfilePermissions { get; set; }
            = new List<PermissionProfilePermission>();
    }
}
