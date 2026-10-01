using LV.SignFlow.Application.Common.Interfaces;
using LV.SignFlow.Domain.Entities.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Templates
{
    public interface ITemplateRepository:IRepository<Template>
    {

        Task<IReadOnlyList<Template>> GetAllForOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);
        Task<Template?> GetByIdForOrganizationAsync(Guid id,Guid organizationId, CancellationToken cancellationToken = default);
    }
}
