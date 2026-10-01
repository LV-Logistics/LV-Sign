using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        Guid OrganizationId { get; }
        string Email { get; }
        bool IsAuthentificated { get; }
    }
}
