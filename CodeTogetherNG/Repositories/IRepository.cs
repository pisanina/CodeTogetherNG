using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories.Entities;
using Microsoft.AspNetCore.Diagnostics;
using System.Collections.Generic;

namespace CodeTogetherNG.Repositories
{
    public interface IRepository
    {
        string Project_OwnerName(int id);
        int RequestsCount(int projectId);
        void Project_Edit(ProjectDetailsViewModel project);
        void ErrorsLog(IExceptionHandlerPathFeature exceptionFeature);
        void NewRequest(int projectId, string userName, string message);
        void SetRequestStatus(int projectId, string memberId, bool accept);
        void NewProject(AddProjectViewModel AddProject, string userName);
        IEnumerable<ProjectsGridViewModel> AllProjects();
        IEnumerable<ProjectsGridViewModel> SearchProject(string ToFind, int[] TechList, bool? newMembers, int? state);
        IEnumerable<TechnologyViewModel> Project_Technology();
        IEnumerable<ProjectStateViewModel> Project_States();
        IEnumerable<RequestsListViewModel> RequestsList(int projectId);
        IEnumerable<ProjectsGridViewModel> MappingDataToProjectsGrid(IEnumerable<ProjectGridEntity> grid);
        ProjectDetailsViewModel MappingDataToProjectDetails(List<ProjectEntity> grid, int requestsCount);
        ProjectDetailsViewModel Project_Details(int IdToFind);
    }
}