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
        public ViewResult AddProject(AddProjectViewModel addProject)
        {
            try
            {
                repo.NewProject(addProject);
            }
            catch (System.Exception e)
            {
                if (e.Message.Contains("Violation of UNIQUE KEY constraint 'UC_Project_Title'."))
                {
                    ModelState.AddModelError("Title", "Sorry there is alredy project with that title");
                    return AddProject();
                }
                else throw;
            }
            return ShowProjectsGrid();
        }

        public ViewResult ShowProjectsGrid()
        {
            return View("ProjectsGrid", repo.AllProjects());
        }

        [HttpGet]
        public ViewResult SearchProjectGrid(string search)
        {
            return View("ProjectsGrid", repo.SearchProject(search));
        }

        [HttpGet]
        public ViewResult ProjectDetails(int id)
        {
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectDetails", repo.Project_Details(id));
        }

        [HttpGet]
        public ViewResult TechnologiesList()
        {
            return View("TechnologiesList", repo.Project_Technology());
        }
    }
}