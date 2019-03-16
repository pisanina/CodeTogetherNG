using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories.Entities;
using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Collections.Generic;

namespace CodeTogetherNG.Repositories
{
    public interface IRepository
    {
        void DeleteProject(int id);
        string Project_OwnerName(int id);
        int RequestsCount(int projectId);
        void DeleteTechnologyLevel(int id);
        void SetRequestStatus(int id, bool accept);
        void Project_Edit(ProjectDetailsViewModel project);
        void ErrorsLog(IExceptionHandlerPathFeature exceptionFeature);
        void NewRequest(int projectId, string userName, string message);
        void NewProject(AddProjectViewModel AddProject, string userName);
        void AddTechnologyLevel(string userName, int techId, int techLevel);
        Tuple<bool, string> GetMembershipState(int projectId, string userName);
        Tuple<bool, string> MappingToMembership(MembershipStateEntity membershipStateEntity);
        IEnumerable<string> UsersList();
        IEnumerable<ProjectsGridViewModel> AllProjects();
        IEnumerable<ProjectStateViewModel> Project_States();
        IEnumerable<TechnologyViewModel> Project_Technology();
        IEnumerable<ProfileProjectRowViewModel> GetProjectsTitleUserInvolve(string userId);
        IEnumerable<RequestsListViewModel> RequestsList(int projectId);
        IEnumerable<ProfileSkillRowViewModel> GetMemberSkills(string userName);
        IEnumerable<ProjectsGridViewModel> SearchProject(string ToFind, int[] TechList, bool? newMembers, int? state);
        IEnumerable<ProjectsGridViewModel> MappingDataToProjectsGrid(IEnumerable<ProjectGridEntity> grid);
        ProjectDetailsViewModel MappingDataToProjectDetails(List<ProjectEntity> grid, int requestsCount, List<string> membersList);
        ProjectDetailsViewModel Project_Details(int IdToFind);
    }
}