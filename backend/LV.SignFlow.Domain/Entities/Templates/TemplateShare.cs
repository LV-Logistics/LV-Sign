using LV.SignFlow.Domain.Enums.TemplateEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Templates
{
    public class TemplateShare
    {
        public Guid Id { get; set; }

        public Guid TemplateId { get; set; }

  
        public Guid? SharedWithUserId { get; set; }

        public Guid? SharedWithDepartmentId { get; set; }

        public TemplateAccessLevel AccessLevel { get; set; }
            = TemplateAccessLevel.Use;

        public Guid SharedByUserId { get; set; }

        public DateTimeOffset SharedAt { get; set; }


        public Template Template { get; set; } = null!;
    }
}
