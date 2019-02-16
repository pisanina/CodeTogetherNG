using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Models
{
    public class ProjectDetailsViewModel
    {
        public int ID;
        public string Title;
        public string Description;
        public DateTimeOffset CreationDate;
        public List<TechnologyViewModel> Technologies;
    }
}
