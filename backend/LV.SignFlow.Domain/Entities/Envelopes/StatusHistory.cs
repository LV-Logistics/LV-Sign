using LV.SignFlow.Domain.Enums.EnvelopeEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Envelopes
{
    public class StatusHistory
    {
        public Guid Id { get; set; }

        public Guid EnvelopeId { get; set; }

        public EnvelopeStatus? PreviousStatus { get; set; }

        public EnvelopeStatus NewStatus { get; set; }

        public Guid? ChangedByUserId { get; set; }

        public string? Reason { get; set; }

        public DateTimeOffset ChangedAt { get; set; }


        public Envelope Envelope { get; set; } = null!;
    }
}
