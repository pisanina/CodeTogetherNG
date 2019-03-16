using CodeTogetherNG.Models;
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

        [HttpGet]
        public ViewResult ShowUserProfile(string userName)
        {
            if(string.IsNullOrEmpty(userName))
            { userName = this.User.Identity.Name; }

            if (this.User.Identity.Name == userName)
                ViewBag.IsOwner = true;
            else
                ViewBag.IsOwner = false;

            ProfileViewModel profile = new ProfileViewModel();
            profile.SkillList = repo.GetMemberSkills(userName);
            profile.ProjectList = repo.GetProjectsTitleUserInvolve(userName);

            ViewBag.TechList = repo.Project_Technology();
            return View("Profile", profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfileAdd(int techId, int level)
        {
            ViewBag.TechList = repo.Project_Technology();
            repo.AddTechnologyLevel(this.User.Identity.Name, techId, level);
            return RedirectToAction("ShowUserProfile", "Member");
        }

        [Authorize]
        public ActionResult UserProfileDeleteTechnology(int id)
        {
            ViewBag.TechList = repo.Project_Technology();
            repo.DeleteTechnologyLevel(id);
            return RedirectToAction("ShowUserProfile", "Member");
        }
    }
}