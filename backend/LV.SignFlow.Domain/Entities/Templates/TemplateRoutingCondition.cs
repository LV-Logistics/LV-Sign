using LV.SignFlow.Domain.Enums.TemplateEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Templates
{
    public class TemplateRoutingCondition
    {
        public Guid Id { get; set; }

        public Guid TemplateRoutingRuleId { get; set; }

        public Guid SourceTemplateFieldId { get; set; }

        public ConditionOperator Operator { get; set; }

        public string? ComparisonValue { get; set; }


        public TemplateRoutingRule TemplateRoutingRule{ get; set; } = null!;

        public TemplateField SourceTemplateField{ get; set; } = null!;
    }
}
