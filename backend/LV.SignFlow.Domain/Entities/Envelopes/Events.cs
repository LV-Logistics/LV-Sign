using LV.SignFlow.Domain.Enums.EnvelopeEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Envelopes
{
    public class Events
    {
        public Guid Id { get; set; }

        public Guid EnvelopeId { get; set; }

        public EnvelopeEventType EventType { get; set; }

        public Guid? UserId { get; set; }

        public Guid? RecipientId { get; set; }

        public DateTimeOffset OccurredAt { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public string? MetadataJson { get; set; }


        public Envelope Envelope { get; set; } = null!;

        public Recipients? Recipient { get; set; }
    }
}
