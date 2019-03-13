using CodeTogetherNG.Controllers;
using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Principal;

namespace CodeTogetherNGTests
{
    [TestFixture]
    public class ProjectController_Tests
    {
        //deklarujemy controlera ktorego bedziemy testowac
        private ProjectController _projectController;

        //deklaracje dependencies controllera
        private IRepository _repository;

        [SetUp]
        public void CreateControllerForTests()
        {
            //podstawiamy faki pod dependencies
            _repository = A.Fake<IRepository>();

            //tworzymy controlera do trstow z fakami
            _projectController = new ProjectController(_repository);

            SetUpAFakeUserIdentity();
        }

        private void SetUpAFakeUserIdentity()
        {
            var identity = new GenericIdentity("");
            var principal = new GenericPrincipal(identity, new[] { "user" } );

            _projectController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        [Test]
        public void AddProjectTest()
        {
            //Arrange - pierwsza faza unit testa
            //AddProject metoda wymaga parametru AddProjectViewModel, wiec tworzymy jakeigos dla testow
            AddProjectViewModel addProjectViewModel = new AddProjectViewModel
            {
                Title       = "unit test title",
                Description = "unit test description"
            };
            //Act - druga faza unit testu
            //wykonujemy metode ktora testujemy
            var result = _projectController.AddProject(addProjectViewModel);

            //Assert - trzecia faza unit testu - weryfikacja
            //weryfikujemy ze metoda NewProject faka dla repository byla uruchomiona z paramterem ktory jest rowny zmiennej addProjectViewModel. I zostala uruchomiona tylko raz.
            A.CallTo(() =>
                _repository.NewProject(A<AddProjectViewModel>.That.Matches(a =>
                a == addProjectViewModel), A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);

            var actionResult = (RedirectToActionResult)result;

            Assert.AreEqual("Project", actionResult.ControllerName);
            Assert.AreEqual("ShowProjectsGrid", actionResult.ActionName);
        }

        [Test]
        public void ProjectsGridTest()
        {
            //Arrange
            List<ProjectsGridViewModel> projectList = new List<ProjectsGridViewModel>
            {
                new ProjectsGridViewModel
                {
                    Title       = "first unit test project title",
                    Description = "First unit test project description"
                }
            };
            A.CallTo(() =>
                _repository.AllProjects()).Returns(projectList);

            //ACT
            var result = _projectController.ShowProjectsGrid();

            //Assert

            A.CallTo(() =>
               _repository.AllProjects()).MustHaveHappened(Repeated.Exactly.Once);

            // Assert.NotNull(result);
            var viewResult = (ViewResult)result;
            Assert.AreEqual("ProjectsGrid", viewResult.ViewName);
        }

        [Test]
        public void DetailsViewTest()
        {
            ProjectDetailsViewModel project = new ProjectDetailsViewModel
            {
                ID          = 1,
                Title       = "unit test for Search project title ",
                Description = "unit test for Search project description"
            };

            A.CallTo(() =>
              _repository.Project_Details(A<int>.That.Matches(i => i == 1))).Returns(project);

            //Act
            var result = _projectController.ProjectDetails(1);

            //Assert
            A.CallTo(() =>
              _repository.Project_Details(1)).MustHaveHappened(Repeated.Exactly.Once);

            var viewResult = (ViewResult)result;
            Assert.AreEqual("ProjectDetails", viewResult.ViewName);
            Assert.AreEqual(project, viewResult.Model);
        }

        [Test]
        public void SearchTest()
        {
            List<ProjectsGridViewModel> projectList = new List<ProjectsGridViewModel>
            {
                new ProjectsGridViewModel
                {
                    Title       = "unit test for Search project title ",
                    Description = "unit test for Search project description"
                }
            };

            A.CallTo(() =>
            _repository.SearchProject(A<string>.That.Matches(i => i == "Search"), A<int[]>.Ignored, A<bool?>.Ignored, A<int>.Ignored)).Returns(projectList);

            var result = _projectController.SearchProjectGrid("Search", new int[] {1,2}, null, 1);

            A.CallTo(() =>
               _repository.SearchProject("Search", A<int[]>.Ignored, A<bool?>.Ignored, A<int>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

            var viewResult = (ViewResult)result;
            Assert.AreEqual("ProjectsGrid", viewResult.ViewName);
            Assert.AreEqual(projectList, viewResult.Model);
        }
    }
}