using CodeTogetherNG.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Collections.Generic;

namespace CodeTogetherNG.Repositories
{
    public interface IRepository
    {
        IEnumerable<ProjectsGridViewModel> AllProjects();
        IEnumerable<ProjectsGridViewModel> SearchProject(string ToFind);
        void NewProject(AddProjectViewModel AddProject);
        void ErrorsLog(IExceptionHandlerPathFeature exceptionFeature);
        ProjectDetailsViewModel Project_Details(int IdToFind);
        IEnumerable<string> Project_Technology();
    }
}