using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Templates
{
    public class TemplateRoutingRuleSet
    {
        public Guid Id { get; set; }

        public Guid TemplateVersionId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public TemplateVersion TemplateVersion { get; set; } = null!;

        public ICollection<TemplateRoutingRule> Rules { get; set; }
            = new List<TemplateRoutingRule>();
    }
}
