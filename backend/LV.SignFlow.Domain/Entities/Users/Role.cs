
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Users
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsSystemRole { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();

        public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();
    }
}
