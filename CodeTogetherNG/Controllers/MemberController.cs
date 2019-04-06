using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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

            ViewBag.UserName = userName;
            ProfileViewModel profile = new ProfileViewModel();
            profile.SkillList = repo.GetMemberSkills(userName);
            profile.ProjectList = repo.GetProjectsTitleUserInvolve(userName);
            profile.ITRoleList = repo.GetUserITRoles(userName);

            ViewBag.TechList = repo.Project_Technology();
            ViewBag.ITRoleList = repo.ITRoleList();
            return View("Profile", profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfileAddTech(int techId, int level)
        {
            repo.AddTechnologyLevel(this.User.Identity.Name, techId, level);
            
            return RedirectToAction("ShowUserProfile", "Member");
        }

        [Authorize]
        public ActionResult UserProfileDeleteTechnology(int id)
        {
            repo.DeleteTechnologyLevel(id);
            return RedirectToAction("ShowUserProfile", "Member");
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfileAddITRole(int roleId)
        {
            try
            {
                repo.AddITRole(this.User.Identity.Name, roleId);
            }
            catch (SqlException sqlEX)
            {
                if (!sqlEX.Message.Contains("UC_UserITRole_UserID_RoleID"))
                    throw;
            }

            return RedirectToAction("ShowUserProfile", "Member");
        }

        [Authorize]
        public ActionResult UserProfileDeleteITRole(int roleId)
        {
            repo.DeleteITRole(this.User.Identity.Name, roleId);
            return RedirectToAction("ShowUserProfile", "Member");
        }
    }
}