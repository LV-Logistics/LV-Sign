using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Signing
{
    public class Stamps
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string BlobPath { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Guid CreatedByUserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public User CreatedByUser { get; set; } = null!;
    }
}
