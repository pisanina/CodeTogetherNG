using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public ViewResult AddProject()
        {
            ViewBag.TechList = repo.Project_Technology();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ViewResult AddProject(AddProjectViewModel addProject)
        {
            string userName = string.Empty;

            if (this.User != null)
                userName = this.User.Identity.Name;

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
        public ViewResult SearchProjectGrid(string search, int[] techList, bool? newMembers)
        {
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectsGrid", repo.SearchProject(search, techList, newMembers));
        }

        [HttpGet]
        public ViewResult ProjectDetails(int id)
        {
            ViewBag.States = repo.Project_States();
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectDetails", repo.Project_Details(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectDetails(ProjectDetailsViewModel project)
        {
            if (this.User != null && this.User.Identity.Name == repo.Project_OwnerName(project.ID))
            {
                try
                {
                    repo.Project_Edit(project);
                }
                catch (System.Exception e)
                {
                    if (e.Message.Contains("Violation of UNIQUE KEY constraint 'UC_Project_Title'."))
                    {
                        ModelState.AddModelError("Title", "Sorry there is alredy project with that title");

                        return ProjectDetails(project.ID);
                    }
                    else throw;
                }
                return ProjectDetails(project.ID);
            }
            else
            {
                return RedirectToAction("Account", "Identity/Login");
            }
        }

        [HttpGet]
        public ViewResult TechnologiesList()
        {
            return View("TechnologiesList", repo.Project_Technology());
        }
    }
}