using System;
using System.Collections.Generic;

namespace authentication_service.Models
{
    public class AcctStateViewModel
    {
        public int AcctStateId { get; set; }
        public string AcctStateName { get; set; }
        public string AcctStateDesc { get; set; }

        public List<string> Users { get; set; }
    }
    public partial class AcctState
    {
        public AcctState()
        {
            User = new HashSet<User>();
        }

        public int AcctStateId { get; set; }
        public string AcctStateName { get; set; }
        public string AcctStateDesc { get; set; }

        public ICollection<User> User { get; set; }
    }
}
