using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Templates.Requests
{
    public class UploadTemplateDocumentRequest
    {
        public Stream Stream { get; set; } = Stream.Null;

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }
    }
}
