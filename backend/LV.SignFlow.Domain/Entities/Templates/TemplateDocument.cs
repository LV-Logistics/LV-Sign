using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Templates
{
    public class TemplateDocument
    {
        public Guid Id { get; set; }
        public Guid TemplateVersionId { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string BlobPath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public int? PageCount { get; set; }

        public long FileSize { get; set; }
        public string? FileHash { get; set; }
        public int Order { get; set; }

        public TemplateVersion TemplateVersion { get; set; } = null!;
        public ICollection<TemplateField> TemplateFields { get; set; } = new List<TemplateField>();
    }
}
