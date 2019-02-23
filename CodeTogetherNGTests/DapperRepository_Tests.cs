using CodeTogetherNG.Repositories;
using CodeTogetherNG.Repositories.Entities;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTogetherNGTests
{
    [TestFixture]
    public class DapperRepository_Tests
    {
        private IRepository _repository;
        private IConfiguration _config;

        [SetUp]
        public void CreateRepositoryForTests()
        {
            _config = A.Fake<IConfiguration>();
            _repository = new DapperRepository(_config);
        }

        [Test]
        public void Details_MappingTest()
        {
            ProjectEntity ProjEntity1= new ProjectEntity
            {
                ID = 1,
                Title = "First",
                Description = "Very long Description ",
                TechName = "C#",
                TechnologyId = 5,
                CreationDate = DateTime.Today
            };

            ProjectEntity ProjEntity2= new ProjectEntity
            {
                ID = 1,
                Title = "First",
                Description = "Very long Description ",
                TechName = "C",
                TechnologyId = 3,
                CreationDate = DateTime.Today
            };

            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();
            ListOfProjects.Add(ProjEntity1);
            ListOfProjects.Add(ProjEntity2);

            var Details = _repository.MappingDataToProjectDetails(ListOfProjects);
            Assert.AreEqual("First", Details.Title);
            Assert.AreEqual("Very long Description ", Details.Description);

            Assert.AreEqual(2, Details.Technologies.Count);
            Assert.AreEqual(5, Details.Technologies[0].Id);
            Assert.AreEqual(3, Details.Technologies[1].Id);
            Assert.AreEqual("C#", Details.Technologies[0].TechName);
            Assert.AreEqual("C", Details.Technologies[1].TechName);
        }

        [Test]
        public void Details_MappingTestNoTechnology()
        {
            ProjectEntity ProjEntity1= new ProjectEntity
            {
                ID = 1,
                Title = "First",
                Description = "Very long Description ",
                //TechName = "C#",
                //TechnologyId = 5,
                CreationDate = DateTime.Today
            };

            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();
            ListOfProjects.Add(ProjEntity1);
            var Details = _repository.MappingDataToProjectDetails(ListOfProjects);

            Assert.AreEqual("First", Details.Title);
            Assert.AreEqual("Very long Description ", Details.Description);
            Assert.IsEmpty(Details.Technologies);
        }

        [Test]
        public void Details_MappingTestNoValues()
        {
            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();

            var Details = _repository.MappingDataToProjectDetails(ListOfProjects);

            Assert.Null(Details);
        }

        [Test]
        public void Details_MappingTestNoObject()
        {
            var Details = _repository.MappingDataToProjectDetails(null);

            Assert.Null(Details);
        }

        
        [Test]
        public void Grid_MappingTestNoObject()
        {
            var grid = _repository.MappingDataToProjectsGrid(null);
            Assert.True(grid.Count() == 0);
        }

        [Test]
        public void Grid_MappingTestNoValues()
        {
            IEnumerable<ProjectGridEntity> emptyList = new List<ProjectGridEntity>();
            var grid = _repository.MappingDataToProjectsGrid(emptyList);
            Assert.True(grid.Count() == 0);
        }


        [Test]
        public void Grid_MappingTestOneProjectTwoTechnologies()
        {
            var list = new List<ProjectGridEntity>();

            var project1 = new ProjectGridEntity
            {
                ID = 1,
                Title = "First program",
                Description = "Something doing nothing",
                TechName = "C",
                TechnologyId = 3
            };

            var project2 = new ProjectGridEntity
            { 
                ID = 1,
                Title = "First program",
                Description = "Something doing nothing",
                TechName = "C#",
                TechnologyId = 5
            };

            var project3 = new ProjectGridEntity
            {
                ID = 2,
                Title = "Second program",
                Description = "Something doing nothing but slowly"
            };

            list.Add(project1);
            list.Add(project2);
            list.Add(project3);


            var grid = _repository.MappingDataToProjectsGrid(list);

            var  firstProject = grid.First(a => a.ID == 1);
            var  secondProject = grid.First(a => a.ID == 2);
            Assert.True(grid.Count()==2);
            Assert.True(firstProject.Technologies.Count == 2);
            Assert.True(firstProject.Technologies.First(a =>a.Id==3).TechName=="C");
            Assert.True(firstProject.Technologies.First(a =>a.Id==5).TechName=="C#");
            Assert.True(firstProject.Title == "First program");
            Assert.True(firstProject.Description == "Something doing nothing");

            Assert.True(grid.First(a => a.ID == 2).Technologies.Count == 0);
            Assert.True(secondProject.Title == "Second program");
            Assert.True(secondProject.Description == "Something doing nothing but slowly");

        }
    }
}