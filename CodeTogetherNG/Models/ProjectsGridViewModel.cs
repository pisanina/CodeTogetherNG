using System.Collections.Generic;

namespace CodeTogetherNG.Models
{
    public class ProjectsGridViewModel
    {
        public int ID;
        public int StateId; 
        public string Title;
        public bool NewMembers;
        public string Description;
        public List<TechnologyViewModel> Technologies;
        public string Search { get; set; }
    }
}