using System;
using System.Collections.Generic;
using System.Text;

namespace LV.SignFlow.Domain.Entities.Users
{
    public class PermissionProfilePermission
    {
        public Guid PermissionProfileId { get; set; }

        public Guid PermissionId { get; set; }


        public PermissionProfile PermissionProfile { get; set; } = null!;

        public Permission Permission { get; set; } = null!;
    }
}
