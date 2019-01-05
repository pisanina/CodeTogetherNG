using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeTogetherNG.Models;
using Dapper;
using System.Data.SqlClient;

namespace CodeTogetherNG.Controllers
{
    public class HomeController : Controller
    {
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

            SqlConnection SQLConnect = new SqlConnection(@"Server = DESKTOP-67FEEF1\SQLEXPRESS; Database = CodeTogetherNG; Trusted_Connection = True;");
            SQLConnect.Execute("Insert into Project Values ('"+AddProject.Title+"','"+AddProject.Description+"')"); 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
