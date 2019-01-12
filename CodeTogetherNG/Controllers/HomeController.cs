using CodeTogetherNG.Models;
using Dapper;
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProjectsGrid()
        {
            ViewData["Message"] = "Grid of Projects.";

            SqlConnection SQLConnect =
               new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            var Grid = SQLConnect.Query<ProjectsGridViewModel>("Exec Projects_Get");

            //List<ProjectsGridViewModel> Grid = new List<ProjectsGridViewModel>();
            return View(Grid);
        }
    }
}