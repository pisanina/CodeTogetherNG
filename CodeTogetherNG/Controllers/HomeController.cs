using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CodeTogetherNG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepository repo;

        public HomeController(IConfiguration config, IRepository repo)
        {
            this.configuration = config;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddProject()
        {
            ViewData["Message"] = "Add Project.";

            return View();
        }

        [HttpPost]
        public IActionResult AddProject(AddProjectViewModel AddProject)
        {
            ViewData["Message"] = "Add Project.";

            repo.NewProject(AddProject);
            return ProjectsGrid();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            repo.ErrorsLog(exceptionFeature);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProjectsGrid()
        {
            ViewData["Message"] = "Grid of Projects.";

            return View("ProjectsGrid", repo.AllProjects());
        }

        [HttpPost]
        public IActionResult ProjectGrid(string Search)
        {
            ViewData["Message"] = "Grid Of Projects";

            return View ("ProjectsGrid", repo.SearchProject(Search));
        }
    }
} 