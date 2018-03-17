using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string RoleTitle { get; set; }

        public ICollection<UserRole> UserRole { get; set; }
    }
}
