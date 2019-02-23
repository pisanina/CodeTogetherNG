using System.Collections.Generic;

namespace CodeTogetherNG.Models
{
    public class ProjectsGridViewModel
    {
        public int ID;
        public string Title;
        public string Description;
        public List<TechnologyViewModel> Technologies;
        public string Search { get; set; }
    }
}