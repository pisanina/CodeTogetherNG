using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories.Entities;
using Microsoft.AspNetCore.Diagnostics;
using System.Collections.Generic;

namespace CodeTogetherNG.Repositories
{
    public interface IRepository
    {
        void NewProject(AddProjectViewModel AddProject);
        void ErrorsLog(IExceptionHandlerPathFeature exceptionFeature);
        IEnumerable<ProjectsGridViewModel> AllProjects();
        IEnumerable<ProjectsGridViewModel> SearchProject(string ToFind, int[] TechList);
        ProjectDetailsViewModel Project_Details(int IdToFind);
        IEnumerable<TechnologyViewModel> Project_Technology();
        ProjectDetailsViewModel MappingDataToProjectDetails(List<ProjectEntity> grid);
        IEnumerable<ProjectsGridViewModel> MappingDataToProjectsGrid(IEnumerable<ProjectGridEntity> grid);
    }
}