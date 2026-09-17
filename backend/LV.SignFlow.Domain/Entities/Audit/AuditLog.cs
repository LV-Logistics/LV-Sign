using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Audit
{
    public class AuditLog
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string? EntityType { get; set; }

        public Guid? EntityId { get; set; }

        public string? DetailsJson { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public DateTimeOffset OccurredAt { get; set; }

        public User User { get; set; } = null!;
    }
}
