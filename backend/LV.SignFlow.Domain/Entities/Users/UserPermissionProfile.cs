using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Users
{
    public class UserPermissionProfile
    {
        public Guid UserId { get; set; }

        public Guid PermissionProfileId { get; set; }

        public DateTimeOffset AssignedAt { get; set; }

        public Guid? AssignedByUserId { get; set; }


        public User User { get; set; } = null!;

        public PermissionProfile PermissionProfile { get; set; } = null!;
    }
}
