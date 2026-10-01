using LV.SignFlow.Domain.Enums.TemplateEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Templates.Requests
{
    public class TemplateRecipientRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public RecipientType RecipientType { get; set; } = RecipientType.Signer;
        public int RoutingOrder { get; set; }
        public bool IsRequired { get; set; } = true;

        public Guid? DefaultUserId { get; set; }

        public bool AllowSenderEditRecipient { get; set; } = true;

        public bool AllowSenderDeleteRecipient { get; set; } = true;

        public bool AllowSenderChangeRoutingOrder { get; set; } = true;

    }
}
