using CodeTogetherNG.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Collections.Generic;

namespace CodeTogetherNG.Repositories
{
    public interface IRepository
    {
        IEnumerable<ProjectsGridViewModel> AllProjects();

        void NewProject(AddProjectViewModel AddProject);

        void ErrorsLog(IExceptionHandlerPathFeature exceptionFeature);
    }
}