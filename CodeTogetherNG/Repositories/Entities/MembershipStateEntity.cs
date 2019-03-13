using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Repositories.Entities
{
    public class MembershipStateEntity
    {
        public bool? AddMember            { get; set; }
        public DateTimeOffset? MessageDate { get; set; }
    }
}
