using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Envelopes
{
    public class EnvelopeRoutingRuleSet
    {
        public Guid Id { get; set; }

        public Guid EnvelopeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;


        public Envelope Envelope { get; set; } = null!;

        public ICollection<EnvelopeRoutingRule> Rules { get; set; }
            = new List<EnvelopeRoutingRule>();
    }
}
