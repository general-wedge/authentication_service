using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public List<string> GroupDomain { get; set; }
        public List<string> UserGroup { get; set; }
    }

    public partial class Group
    {
        public Group()
        {
            GroupDomain = new HashSet<GroupDomain>();
            UserGroup = new HashSet<UserGroup>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public ICollection<GroupDomain> GroupDomain { get; set; }
        public ICollection<UserGroup> UserGroup { get; set; }
    }
}
