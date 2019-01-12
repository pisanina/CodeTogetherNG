using CodeTogetherNG.Models;
using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CodeTogetherNG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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

            SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            // SQLConnect.Execute("Insert into Project Values ('"+AddProject.Title+"','"+AddProject.Description+"')");
            SQLConnect.Execute("Exec Project_Add @Title=@T,  @Description=@D",
                new { T = AddProject.Title, D = AddProject.Description });
            return ProjectsGrid();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            SqlConnection SQLConnect =
               new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            SQLConnect.Execute("Exec Logs_Add @ErrorMessage=@E",
                new { E = exceptionFeature.Error.Message });

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProjectsGrid()
        {
            ViewData["Message"] = "Grid of Projects.";

            SqlConnection SQLConnect =
               new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            var Grid = SQLConnect.Query<ProjectsGridViewModel>("Exec Projects_Get");

            return View("ProjectsGrid", Grid);
        }
    }
}