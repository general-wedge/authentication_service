using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public partial class UserRole
    {
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
