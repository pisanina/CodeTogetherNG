using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
        public void MappingTest()
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
        public void MappingTestNotechnology()
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
        public void MappingTestNoValues()
        {
            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();

            var Details = _repository.MappingDataToProjectDetails(ListOfProjects);

            Assert.Null(Details);
        }

        [Test]
        public void MappingTestNoObject()
        {
            var Details = _repository.MappingDataToProjectDetails(null);

            Assert.Null(Details);
        }
    }
}