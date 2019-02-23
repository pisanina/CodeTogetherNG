﻿using CodeTogetherNG.Models;
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

        public void NewProject(AddProjectViewModel addProject)
        {
            DataTable tbTechList = new DataTable();
            tbTechList.Columns.Add("Id", typeof(int));

            using (SqlConnection SQLConnect =
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                if (addProject.TechList != null)
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
                var Grids = SQLConnect.Query<ProjectEntity>("Exec Project_Details @FindId=@Id", new { Id = idToFind }).ToList();
                return MappingDataToProjectDetails(Grids);
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

        public ProjectDetailsViewModel MappingDataToProjectDetails(List<ProjectEntity> grid)
        {
            if (grid != null && grid.Any())
            {
                ProjectDetailsViewModel  ProjectDetails = new ProjectDetailsViewModel
                {
                    ID = grid[0].ID,
                    Title = grid[0].Title,
                    Description = grid[0].Description,
                    CreationDate = grid[0].CreationDate,
                    Technologies = new List<TechnologyViewModel>()
                };

                foreach (var item in grid)
                {
                    if (item.TechnologyId != 0)
                    {
                        ProjectDetails.Technologies.Add(new TechnologyViewModel
                        {
                            Id = item.TechnologyId,
                            TechName = item.TechName
                        });
                    }
                    else break;
                }

                return ProjectDetails;
            }
            else return null;
        }
    }
}