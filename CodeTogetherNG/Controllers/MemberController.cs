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
            
            return View("RequestMember", repo.RequestsList(projectId) );
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

      
        [Authorize]
        public ViewResult ReactToRequest(int projectId, string memberId, bool accept)
        {
            string userName = string.Empty;

            if (this.User != null)
                userName = this.User.Identity.Name;

            repo.SetRequestStatus(projectId, memberId, accept);
            return View("RequestMember", projectId);
        }

    }
}