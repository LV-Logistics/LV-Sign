using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Users
{
    public class PermissionProfile
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsSystemProfile { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public ICollection<PermissionProfilePermission> Permissions { get; set; }
            = new List<PermissionProfilePermission>();

        public ICollection<UserPermissionProfile> Users { get; set; }
            = new List<UserPermissionProfile>();
    }
}
