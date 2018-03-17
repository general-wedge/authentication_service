using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? LastLoginTimestamp { get; set; }

        public int? AcctStateId { get; set; }

        public string AcctState { get; set; }
        public List<string> UserGroup { get; set; }
        public List<string> UserPermission { get; set; }
        public List<string> UserRole { get; set; }
    }

    public partial class User
    {
        public User()
        {
            UserGroup = new HashSet<UserGroup>();
            UserPermission = new HashSet<UserPermission>();
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Salt { get; set; }
        public int? AcctStateId { get; set; }
        public DateTime? LastLoginTimestamp { get; set; }

        public AcctState AcctState { get; set; }
        public ICollection<UserGroup> UserGroup { get; set; }
        public ICollection<UserPermission> UserPermission { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
