using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public partial class UserGroup
    {
        public int UserGroupId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
    }
}
