using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Enums.TemplateEnums
{
    public enum ConditionOperator
    {
        Equals = 1,
        NotEquals = 2,
        Contains = 3,
        DoesNotContain = 4,
        IsEmpty = 5,
        IsNotEmpty = 6,

        GreaterThan = 7,
        GreaterThanOrEqual = 8,
        LessThan = 9,
        LessThanOrEqual = 10
    }
}
