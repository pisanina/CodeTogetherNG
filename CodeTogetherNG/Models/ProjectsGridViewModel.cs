using System.Collections.Generic;

namespace CodeTogetherNG.Models
{
    public class ProjectsGridViewModel
    {
        public int ID;
        public string Title;
        public bool NewMembers;
        public string Description;
        public List<TechnologyViewModel> Technologies;
    }
}