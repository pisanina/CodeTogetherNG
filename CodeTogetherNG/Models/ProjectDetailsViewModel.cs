using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Models
{
    public class ProjectDetailsViewModel
    {
        public int ID                                 { get; set; }
        public string Title                           { get; set; }
        public string Description                     { get; set; }
        public string CreationDate                    { get; set; }
        [DisplayName("Owner Name")]
        public string OwnerName                       { get; set; }
        [DisplayName("Project State")]
        public int StateId                            { get; set; }
        [DisplayName("Looking for new members?")]
        public bool NewMembers                        { get; set; }
        // public List<TechnologyViewModel> Technologies { get; set; }
        public List<int> TechList { get; set; }
    }
}
