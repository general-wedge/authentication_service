using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public partial class UserPermission
    {
        public int UserPermissionId { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }

        public Permission Permission { get; set; }
        public User User { get; set; }
    }
}
