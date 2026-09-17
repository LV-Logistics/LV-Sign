using LV.SignFlow.Domain.Entities.Envelopes;
using LV.SignFlow.Domain.Entities.Templates;
using LV.SignFlow.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Organizations
{
    public class Organization
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public ICollection<Department> Departments { get; set; }
            = new List<Department>();

        public ICollection<User> Users { get; set; }
            = new List<User>();
        public ICollection<Template> Templates { get; set; }
            = new List<Template>();
        public ICollection<Envelope> Envelopes { get; set; }
            = new List<Envelope>();
    }
}
