﻿using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories.Entities;
using Microsoft.AspNetCore.Diagnostics;
using System;
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
        void SetRequestStatus(int id, bool accept);
        void NewProject(AddProjectViewModel AddProject, string userName);
        Tuple<bool, string> GetMembershipState(int projectId, string userName);
        Tuple<bool, string> MappingToMembership(MembershipStateEntity membershipStateEntity);
        IEnumerable<ProjectsGridViewModel> AllProjects();
        IEnumerable<ProjectsGridViewModel> SearchProject(string ToFind, int[] TechList, bool? newMembers, int? state);
        IEnumerable<TechnologyViewModel> Project_Technology();
        IEnumerable<ProjectStateViewModel> Project_States();
        IEnumerable<RequestsListViewModel> RequestsList(int projectId);
        IEnumerable<ProjectsGridViewModel> MappingDataToProjectsGrid(IEnumerable<ProjectGridEntity> grid);
        ProjectDetailsViewModel MappingDataToProjectDetails(List<ProjectEntity> grid, int requestsCount, List<string> membersList);
        ProjectDetailsViewModel Project_Details(int IdToFind);
    }
}