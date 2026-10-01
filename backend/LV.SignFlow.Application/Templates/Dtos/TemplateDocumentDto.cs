using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Templates.Dtos
{
    public class TemplateDocumentDto
    {
        public Guid Id { get; set; }

        public Guid TemplateVersionId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public int Order { get; set; }

        public int? PageCount { get; set; }
    }
}
