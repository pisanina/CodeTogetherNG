using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Models
{
    public class UsersListViewModel
    {
        public string UserName { get; set; }
        public int OwnerOf     { get; set; }
        public int MemberOf    { get; set; }
        public int BeginnerIn  { get; set; }
        public int AdvancedIn  { get; set; }
        public int ExpertIn    { get; set; }
    }
}
