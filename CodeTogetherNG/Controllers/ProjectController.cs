using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Identity;
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
            var userName = this.User.Identity.Name;

            try
            {
                repo.NewProject(addProject, userName);
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
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectsGrid", repo.AllProjects());
        }

        [HttpGet]
        public ViewResult SearchProjectGrid(string search, int[] TechList)
        {
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectsGrid", repo.SearchProject(search, TechList));
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