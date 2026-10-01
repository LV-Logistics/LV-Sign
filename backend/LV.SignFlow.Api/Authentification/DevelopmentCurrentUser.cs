using LV.SignFlow.Application.Common.Interfaces;

namespace LV.SignFlow.Api.Authentification
{
    public class DevelopmentCurrentUser : ICurrentUser
    {
        public Guid UserId => DevelopmentUserId;

        public Guid OrganizationId => DevelopmentOrganizationId;

        public string Email => "example@lv-logistics.com";

        public bool IsAuthentificated => true;
        public static readonly Guid DevelopmentUserId =
        Guid.Parse("40000000-0000-0000-0000-000000000001");

        public static readonly Guid DevelopmentOrganizationId =
            Guid.Parse("40000000-0000-0000-0000-000000000002");
    }
}
