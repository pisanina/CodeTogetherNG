using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Models
{
    public class ProjectDetailsViewModel
    {
        public int ID;
        public string Title;
            [DisplayName("Owner Name")]
        public string OwnerName { get; set; }
        public string Description;
        public string CreationDate;
        public List<TechnologyViewModel> Technologies;
    }
}
