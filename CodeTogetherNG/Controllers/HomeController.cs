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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProject(AddProjectViewModel AddProject)
        {
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
            return View("ProjectsGrid", repo.AllProjects());
        }

        [HttpGet]
        public IActionResult ProjectGrid(string Search)
        {
            return View("ProjectsGrid", repo.SearchProject(Search));
        }

        [HttpGet]
        public IActionResult ProjectDetails(int Id)
        {
            return View("ProjectDetails", repo.Project_Details(Id));
        }
    }
}