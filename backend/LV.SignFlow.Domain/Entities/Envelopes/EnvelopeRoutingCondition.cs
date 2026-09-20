using LV.SignFlow.Domain.Enums.TemplateEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Envelopes
{
    public class EnvelopeRoutingCondition
    {
        public Guid Id { get; set; }

        public Guid EnvelopeRoutingRuleId { get; set; }

        public Guid SourceEnvelopeFieldId { get; set; }

        public ConditionOperator Operator { get; set; }

        public string? ComparisonValue { get; set; }


        public EnvelopeRoutingRule EnvelopeRoutingRule { get; set; } = null!;

        public Fields SourceEnvelopeField { get; set; } = null!;
    }
}
