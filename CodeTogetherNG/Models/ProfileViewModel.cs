using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Models
{
    public class ProfileViewModel
    {
        public IEnumerable<ProfileSkillRowViewModel> SkillList { get; set; }
        public IEnumerable<ProfileProjectRowViewModel> ProjectList    { get; set; }
    }
}
