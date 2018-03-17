using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public partial class Domain
    {
        public Domain()
        {
            GroupDomain = new HashSet<GroupDomain>();
        }

        public int DomainId { get; set; }
        public string DomainName { get; set; }

        public ICollection<GroupDomain> GroupDomain { get; set; }
    }
}
