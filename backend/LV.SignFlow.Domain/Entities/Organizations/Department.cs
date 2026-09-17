using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Organizations
{
    public class Department
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public Organization Organization { get; set; } = null!;

        public ICollection<User> Users { get; set; }
            = new List<User>();
    }
}
