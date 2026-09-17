using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Enums.EnvelopeEnums
{
    public enum RecipientStatus
    {
        Pending=1,
        Sent=2,
        Viewed=3,
        Completed=4,
        Declined=5,
        WaitingForCondition=6,
        Skipped=7
    }
}
