using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories.Entities;
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
                var grid = SQLConnect.Query<ProjectGridEntity>("Exec Projects_Get");
                return MappingDataToProjectsGrid(grid);
            }
        }

        public IEnumerable<ProjectsGridViewModel> MappingDataToProjectsGrid(IEnumerable<ProjectGridEntity> grid)
        {
            var list = new List<ProjectsGridViewModel>();

            if (grid != null)
            {
                foreach (var item in grid)
                {
                    ProjectsGridViewModel project;

                    if (list.Any(a => a.ID == item.ID))
                    {
                        project = list.Find(a => a.ID == item.ID);
                    }
                    else
                    {
                        project = new ProjectsGridViewModel
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Description = item.Description,
                            NewMembers = item.NewMembers,
                            Technologies = new List<TechnologyViewModel>()
                        };
                        list.Add(project);
                    }
                    if (item.TechnologyId != 0)
                    {
                        project.Technologies.Add(new TechnologyViewModel
                        {
                            TechName = item.TechName,
                            Id = item.TechnologyId
                        });
                    }
                }
            }

            return list;
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


        public void NewProject(AddProjectViewModel addProject, string userName)
        {
            DataTable tbTechList = new DataTable();
            tbTechList.Columns.Add("Id", typeof(int));

            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                if (addProject.TechList != null)
                {
                    addProject.TechList.ForEach(x => tbTechList.Rows.Add(x));

                    SQLConnect.Execute("Exec Project_Add @Title=@T,  @Description=@D, @TechList=@L, @UserName=@U, @NewMembers=@M",
                            new
                            {
                                T = addProject.Title,
                                D = addProject.Description,
                                L = tbTechList.AsTableValuedParameter("TechnologyList"),
                                U = userName,
                                M = addProject.NewMembers
                            });
                }
                else
                {
                    SQLConnect.Execute("Exec Project_Add @Title=@T,  @Description=@D, @UserName=@U, @NewMembers=@M",
                              new { T = addProject.Title, D = addProject.Description, U = userName, M = addProject.NewMembers });
                }
            }
        }


        public IEnumerable<ProjectsGridViewModel> SearchProject(string toFind, int[] chosenTechs, bool? newMembers)
        {
            DataTable dataTableTechList = new DataTable();
            dataTableTechList.Columns.Add("Id", typeof(int));
            IEnumerable<ProjectGridEntity> grid;

            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                if (chosenTechs != null && chosenTechs.Count() > 0)
                {
                    for (int i = 0; i < chosenTechs.Count(); i++)
                    {
                        dataTableTechList.Rows.Add(chosenTechs[i]);
                    }
                }

                grid = SQLConnect.Query<ProjectGridEntity>("Exec Project_Search @ToFind=@S, @TechList=@L, @newMembers = @M",
                    new { S = toFind, L = dataTableTechList.AsTableValuedParameter("TechnologyList"), M = newMembers });
            }
            return MappingDataToProjectsGrid(grid);
        }


        public ProjectDetailsViewModel Project_Details(int idToFind)
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var Grids = SQLConnect.Query<ProjectEntity>("Exec Project_Details @FindId=@Id", new { Id = idToFind }).ToList();
                return MappingDataToProjectDetails(Grids);
            }
        }


        public void Project_Edit(ProjectDetailsViewModel project)
        {
            DataTable chosenTechs = new DataTable();
            chosenTechs.Columns.Add("Id", typeof(int));

            using (SqlConnection SQLConnect =
               new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                if (project.TechList != null && project.TechList.Count() > 0)
                {
                    for (int i = 0; i < project.TechList.Count; i++)
                    {
                        chosenTechs.Rows.Add(project.TechList[i]);
                    }
                }

                SQLConnect.Execute("Exec Project_Edit @id=@I, @title=@T, @description=@D," +
                                    "@techList=@L, @newMembers = @M, @stateId = @S",
                                     new {
                                             I = project.ID,
                                             T = project.Title,
                                             D = project.Description,
                                             L = chosenTechs.AsTableValuedParameter("TechnologyList"),
                                             M = project.NewMembers,
                                             S = project.StateId
                                         });
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

        public IEnumerable<ProjectStateViewModel> Project_States()
        {
            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var StateList = SQLConnect.Query<ProjectStateViewModel>("Exec State_List");
                return StateList;
            }
        }

        public string Project_OwnerName(int id)
        {
            using (SqlConnection SQLConnect =
               new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var OwnerName = SQLConnect.QuerySingle<string>("Exec ProjectOwnerName @Id=@I", new {I = id});
                return OwnerName;
            }
        }

        public ProjectDetailsViewModel MappingDataToProjectDetails(List<ProjectEntity> grid)
        {
            if (grid != null && grid.Any())
            {
                ProjectDetailsViewModel  ProjectDetails = new ProjectDetailsViewModel
                {
                    ID           = grid[0].ID,
                    Title        = grid[0].Title,
                    StateId      = grid[0].StateId,
                    OwnerName    = grid[0].UserName,
                    NewMembers   = grid[0].NewMembers,
                    Description  = grid[0].Description,
                    CreationDate = grid[0].CreationDate,
                   // Technologies = new List<TechnologyViewModel>()
                    TechList = new List<int>()
                };

                foreach (var item in grid)
                {
                    if (item.TechnologyId != 0)
                    {
                        ProjectDetails.TechList.Add(item.TechnologyId);
                    }
                    else break;
                }

                return ProjectDetails;
            }
            else return null;
        }
    }
}