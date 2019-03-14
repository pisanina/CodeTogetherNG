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

        [Authorize]
        public ViewResult RequestMember(int projectId)
        {
            return View("RequestMember", repo.RequestsList(projectId));
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewRequest(int projectId, string message)
        {
            repo.NewRequest(projectId, this.User.Identity.Name, message);
            return RedirectToAction("ShowProjectsGrid", "Project");
        }

        [Authorize]
        public ActionResult ReactToRequest(int id, bool accept, int projectId)
        {
            repo.SetRequestStatus(id, accept);
            return RedirectToAction("RequestMember", "Member", new { projectId = projectId });
        }
    }
}