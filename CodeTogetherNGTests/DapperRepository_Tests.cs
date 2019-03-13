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
                UserName = "TestUser@a.com",
                NewMembers = true,
                CreationDate = DateTime.Today.ToString("dd/MM/yyyy")
            };

            ProjectEntity ProjEntity2= new ProjectEntity
            {
                ID = 1,
                Title = "First",
                Description = "Very long Description ",
                TechName = "C",
                TechnologyId = 3,
                UserName = "TestUser@a.com",
                NewMembers = true,
                CreationDate = DateTime.Today.ToString("dd/MM/yyyy")
            };

            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();
            ListOfProjects.Add(ProjEntity1);
            ListOfProjects.Add(ProjEntity2);
            List<string> MembersList = new List<string>();

            var Details = _repository.MappingDataToProjectDetails(ListOfProjects, 0, MembersList);
            Assert.AreEqual("First", Details.Title);
            Assert.AreEqual("Very long Description ", Details.Description);
            Assert.AreEqual("TestUser@a.com", Details.OwnerName);

            Assert.AreEqual(2, Details.TechList.Count);
            Assert.AreEqual(5, Details.TechList[0]);
            Assert.AreEqual(3, Details.TechList[1]);
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
                UserName = "TestUser@a.com",
                CreationDate = DateTime.Today.ToString("dd/MM/yyyy")
            };

            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();
            ListOfProjects.Add(ProjEntity1);
            List<string> MembersList = new List<string>();
            var Details = _repository.MappingDataToProjectDetails(ListOfProjects, 0, MembersList);

            Assert.AreEqual("First", Details.Title);
            Assert.AreEqual("Very long Description ", Details.Description);
            Assert.IsEmpty(Details.TechList);
        }

        [Test]
        public void Details_MappingTestNoValues()
        {
            List<ProjectEntity> ListOfProjects = new List<ProjectEntity>();
            List<string> MembersList = new List<string>();

            var Details = _repository.MappingDataToProjectDetails(ListOfProjects, 0, MembersList);

            Assert.Null(Details);
        }

        [Test]
        public void Details_MappingTestNoObject()
        {
            List<string> MembersList = new List<string>();
            var Details = _repository.MappingDataToProjectDetails(null, 0, MembersList);

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
                NewMembers = true,
                TechnologyId = 3
            };

            var project2 = new ProjectGridEntity
            {
                ID = 1,
                Title = "First program",
                Description = "Something doing nothing",
                TechName = "C#",
                NewMembers = true,
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
            Assert.True(grid.Count() == 2);
            Assert.True(firstProject.Technologies.Count == 2);
            Assert.True(firstProject.Technologies.First(a => a.Id == 3).TechName == "C");
            Assert.True(firstProject.Technologies.First(a => a.Id == 5).TechName == "C#");
            Assert.True(firstProject.Title == "First program");
            Assert.True(firstProject.Description == "Something doing nothing");
            Assert.True(firstProject.NewMembers == true);

            Assert.True(grid.First(a => a.ID == 2).Technologies.Count == 0);
            Assert.True(secondProject.Title == "Second program");
            Assert.True(secondProject.Description == "Something doing nothing but slowly");
            Assert.True(secondProject.NewMembers == false);
        }

        [Test]
        public void MappingToMembership_AlreadyMember()
        {
            var membership = new MembershipStateEntity
            {
                AddMember = true,
                MessageDate = DateTimeOffset.Now
            };

            var tuple = _repository.MappingToMembership(membership);

            Assert.False(tuple.Item1);
            Assert.True(tuple.Item2 == "");
        }

        [Test]
        public void MappingToMembership_New()
        {
            
            var tuple = _repository.MappingToMembership(null);

            Assert.True(tuple.Item1);
            Assert.True(tuple.Item2 == "");
        }

        [Test]
        public void MappingToMembership_RequestPending()
        {
            var membership = new MembershipStateEntity
            {
                AddMember = null,
                MessageDate = DateTimeOffset.Now
            };

            var tuple = _repository.MappingToMembership(membership);

            Assert.False(tuple.Item1);
            Assert.True(tuple.Item2 == "Your request is pending");
        }

        [Test]
        public void MappingToMembership_DeclineWaitedEnough()
        {
            var membership = new MembershipStateEntity
            {
                AddMember = true,
                MessageDate = DateTimeOffset.Now.AddMonths(-1)
            };

            var tuple = _repository.MappingToMembership(membership);

            Assert.False(tuple.Item1);
            Assert.True(tuple.Item2 == "");
        }

        [Test]
        public void MappingToMembership_DeclineMustWait()
        {
            var membership = new MembershipStateEntity
            {
                AddMember = false,
                MessageDate = DateTimeOffset.Now.AddDays(-10)
            };

            var tuple = _repository.MappingToMembership(membership);

            Assert.False(tuple.Item1);
            Assert.True(tuple.Item2 == "Your unable to send a join request until " +
                    membership.MessageDate.Value.AddMonths(1).ToString("dd/MM/yyyy"));
        }
    }
}