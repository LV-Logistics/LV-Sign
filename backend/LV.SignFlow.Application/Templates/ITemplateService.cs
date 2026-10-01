using LV.SignFlow.Application.Templates.Dtos;
using LV.SignFlow.Application.Templates.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Templates
{
    public interface ITemplateService
    {
        Task<IReadOnlyList<TemplateListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TemplateDetailsDto?> GetByIdAsync(Guid Id,CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(CreateTemplateRequest request,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TemplateRecipientRoleDto>> GetRecipientRoleAsync(Guid templateId,CancellationToken cancellationToken = default);
        Task<TemplateRecipientRoleDto> AddRecipientRoleAsync(Guid templateId,TemplateRecipientRoleRequest request,CancellationToken cancellationToken = default);
        Task<TemplateDocumentDto> UploadDocumentAsync(Guid templateId, UploadTemplateDocumentRequest request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TemplateFieldDto>> GetFieldsAsync(Guid templateId,CancellationToken cancellationToken = default);
        Task<TemplateFieldDto> AddFieldsAsync(Guid templateId,TemplateFieldRequest templateFieldRequest,CancellationToken cancellationToken = default);
        Task<TemplateFieldDto> UpdateTemplateFieldAsync(Guid templateId,Guid fieldId, TemplateFieldRequest request, CancellationToken cancellationToken);
        Task DeleteTemplateFieldAsync(Guid templateId, Guid fieldId, CancellationToken cancellationToken);
        Task<TemplateRecipientRoleDto> UpdateTemplateRecipientRoledAsync(Guid templateId, Guid fieldId, TemplateRecipientRoleRequest request, CancellationToken cancellationToken);
        Task DeleteTemplateRecipientRoleAsync(Guid templateId, Guid fieldId, CancellationToken cancellationToken);

    }
}
