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
        public ActionResult AddProject(AddProjectViewModel addProject)
        {
            try
            {
                repo.NewProject(addProject, this.User.Identity.Name);
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

            return RedirectToAction("ShowProjectsGrid", "Project");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteProject(int projectId)
        {
            repo.DeleteProject(projectId);
            return RedirectToAction("ShowProjectsGrid", "Project");
        }


        public ViewResult ShowProjectsGrid()
        {
            ViewBag.States = repo.Project_States();
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectsGrid", repo.AllProjects());
        }

        [HttpGet]
        public ViewResult SearchProjectGrid(string search, int[] techList, bool? newMembers, int? state)
        {
            ViewBag.States = repo.Project_States();
            ViewBag.TechList = repo.Project_Technology();
            return View("ProjectsGrid", repo.SearchProject(search, techList, newMembers, state));
        }

        [HttpGet]
        public ViewResult ProjectDetails(int id)
        {
            ViewBag.States = repo.Project_States();
            ViewBag.TechList = repo.Project_Technology();
            ViewBag.Membership = repo.GetMembershipState(id, this.User.Identity.Name);
            return View("ProjectDetails", repo.Project_Details(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectDetails(ProjectDetailsViewModel project)
        {
            if (this.User.Identity.Name == repo.Project_OwnerName(project.ID))
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
                return RedirectToAction("ShowProjectsGrid", "Project");
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