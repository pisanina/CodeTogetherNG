using CodeTogetherNG.Models;
using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CodeTogetherNG.Repositories
{
    public class DapperRepository : IRepository
    {
        private readonly IConfiguration configuration;

        public DapperRepository(IConfiguration config)
        {
            this.configuration = config;
        }

        public IEnumerable<ProjectsGridViewModel> AllProjects()
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var Grid = SQLConnect.Query<ProjectsGridViewModel>("Exec Projects_Get");
                return Grid;
            }
        }

        public void ErrorsLog(IExceptionHandlerPathFeature exceptionFeature)
        {
            using (SqlConnection SQLConnect =
                 new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                SQLConnect.Execute("Exec Logs_Add @ErrorMessage=@E",
                    new { E = exceptionFeature.Error.Message });
            }
        }

        public void NewProject(AddProjectViewModel AddProject)
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                // SQLConnect.Execute("Insert into Project Values ('"+AddProject.Title+"','"+AddProject.Description+"')");
                SQLConnect.Execute("Exec Project_Add @Title=@T,  @Description=@D",
                        new { T = AddProject.Title, D = AddProject.Description });
            }
        }

        public IEnumerable<ProjectsGridViewModel> SearchProject(string ToFind)
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var Grid = SQLConnect.Query<ProjectsGridViewModel>("Exec Search_Project @ToFind=@S", new { S = ToFind });
                return Grid;
            }
        }

        public ProjectDetailsViewModel Project_Details(int IdToFind)
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var Grid = SQLConnect.QuerySingle<ProjectDetailsViewModel>("Exec Project_Details @FindId=@Id", new { Id = IdToFind });
                return Grid;
            }
        }
    }
}