using CodeTogetherNG.Models;
using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


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

        public void NewProject(AddProjectViewModel addProject)
        {
            DataTable tbTechList = new DataTable();
            tbTechList.Columns.Add("Id", typeof(int));
           


            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            { if ( addProject.TechList != null)
                {
                    addProject.TechList.ForEach(x => tbTechList.Rows.Add(x));

                    SQLConnect.Execute("Exec Project_Add @Title=@T,  @Description=@D, @TechList=@L",
                            new { T = addProject.Title, D = addProject.Description, L = tbTechList.AsTableValuedParameter("TechnologyList") });
                }
                else
                {
                    SQLConnect.Execute("Exec Project_Add @Title=@T,  @Description=@D",
                              new { T = addProject.Title, D = addProject.Description });
                }
            }
        }

        public IEnumerable<ProjectsGridViewModel> SearchProject(string toFind)
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var Grid = SQLConnect.Query<ProjectsGridViewModel>("Exec Project_Search @ToFind=@S", new { S = toFind });
                return Grid;
            }
        }

        public ProjectDetailsViewModel Project_Details(int idToFind)
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var Grid = SQLConnect.QuerySingle<ProjectDetailsViewModel>("Exec Project_Details @FindId=@Id", new { Id = idToFind });
                return Grid;
            }
        }

        public IEnumerable<TechnologyViewModel> Project_Technology()
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var TechnologyList = SQLConnect.Query<TechnologyViewModel>("Exec Technology_List");
                return TechnologyList;
            }
        }
    }
}