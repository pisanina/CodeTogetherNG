using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogetherNG.Controllers
{
    public class MemberController : Controller
    {
        private readonly IRepository repo;

        public MemberController(IRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewRequest(int projectId, string message)
        {
            string userName = string.Empty;

            if (this.User != null)
                userName = this.User.Identity.Name;

            repo.NewRequest(projectId, userName, message);
            return RedirectToAction("ShowProjectsGrid", "Project");
        }
    }
}