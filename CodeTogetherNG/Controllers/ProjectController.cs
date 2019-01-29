using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogetherNG.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IRepository repo;

        public ProjectController(IRepository repo)
        {
            this.repo = repo;
        }

        public ViewResult AddProject()
        {
            ViewBag.TechList = repo.Project_Technology();
            return View();
        }

        [HttpPost]
        public ViewResult AddProject(AddProjectViewModel AddProject)
        {
            repo.NewProject(AddProject);
            return ShowProjectsGrid();
        }


        public ViewResult ShowProjectsGrid()
        {
            return View("ProjectsGrid", repo.AllProjects());
        }

        [HttpGet]
        public ViewResult SearchProjectGrid(string Search)
        {
            return View("ProjectsGrid", repo.SearchProject(Search));
        }

        [HttpGet]
        public ViewResult ProjectDetails(int Id)
        {
            return View("ProjectDetails", repo.Project_Details(Id));
        }
    }
}
