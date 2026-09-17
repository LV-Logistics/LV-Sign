using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Enums.EnvelopeEnums
{
    public enum EnvelopeStatus
    {
        Draft=1,
        Sent=2,
        InProgress=3,
        Completed=4,
        Declined=5,
        Voided=6,
        Expired=7
    }
}
