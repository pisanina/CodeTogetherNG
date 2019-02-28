using System;

namespace CodeTogetherNG.Repositories.Entities
{
    public class ProjectEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TechName { get; set; }
        public int TechnologyId { get; set; }
        public string CreationDate { get; set; }
        public string UserName { get; set; }
    }
}