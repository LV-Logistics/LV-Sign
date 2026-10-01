using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Templates.Requests
{
    public class CreateTemplateRequest
    {
        public string Name { get; set; }=string.Empty;
        public string? Description { get; set; }
    }
}
