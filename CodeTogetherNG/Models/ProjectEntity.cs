using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG.Models
{
    public class ProjectEntity
    {
        public int ID                      { get; set; }
        public string Title                { get; set; }
        public string Description          { get; set; }
        public string TechName             { get; set; }
        public int TechnologyId            { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
