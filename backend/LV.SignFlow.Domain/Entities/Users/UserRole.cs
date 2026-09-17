using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Users
{
    public class UserRole
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public DateTimeOffset AssignedAt { get; set; }

        public Guid? AssignedByUserId { get; set; }


        public User User { get; set; } = null!;

        public Role Role { get; set; } = null!;
    }
}
