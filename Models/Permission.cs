using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public class PermissionViewModel
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDesc { get; set; }

        public List<string> UserPermission { get; set; }
    }

    public partial class Permission
    {
        public Permission()
        {
            UserPermission = new HashSet<UserPermission>();
        }

        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDesc { get; set; }

        public ICollection<UserPermission> UserPermission { get; set; }
    }
}
