using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodeTogetherNG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repo;

        public HomeController(IRepository repo)
        {
            this.repo = repo;
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            repo.ErrorsLog(exceptionFeature);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}