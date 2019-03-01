using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Repositories.Entities
{
    public class ProjectGridEntity
    {
        public int ID             { get; set; }
        public string Title       { get; set; }
        public bool NewMembers     { get; set; }
        public string TechName    { get; set; }
        public int TechnologyId   { get; set; }
        public string Description { get; set; }
    }
}
