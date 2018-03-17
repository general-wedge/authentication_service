using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public partial class GroupDomain
    {
        public int GroupDomainId { get; set; }
        public int GroupId { get; set; }
        public int DomainId { get; set; }

        public Domain Domain { get; set; }
        public Group Group { get; set; }
    }
}
