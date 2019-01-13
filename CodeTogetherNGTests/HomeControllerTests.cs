using CodeTogetherNG.Controllers;
using CodeTogetherNG.Models;
using CodeTogetherNG.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;

namespace CodeTogetherNGTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        //deklarujemy controlera ktorego bedziemy testowac
        private  HomeController _homeController;

        //deklaracje dependencies controllera
        private  IConfiguration _config;

        private  IRepository _repository;

        [SetUp]
        public void CreateControllerForTests()
        {
            //podstawiamy faki pod dependencies
            _config = A.Fake<IConfiguration>();
            _repository = A.Fake<IRepository>();

            //tworzymy controlera do trstow z fakami
            _homeController = new HomeController(_config, _repository);
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
            //tworzymy sztuczna liste projektow, by unit test nie probowal laczyc sie z baza danych. Nie chcemy testowac poalczenia do bazy danych czy bazy danych. To unit test tylko dla controllera. I jedynie dla kontrollera. Wiec wszystko inne musi byc mockniete.
            var projectList = new List<ProjectsGridViewModel>
            {
                new ProjectsGridViewModel
                {
                    Title       = "first unit test project title",
                    Description = "First unit test project description"
                }
            };

            //definiujemy jak fake dla repository ma sie zachowac jak controller wykona metode AllProjects
            A.CallTo(() =>
                _repository.AllProjects()).Returns(projectList);

            //Act - druga faza unit testu
            //wykonujemy metode ktora testujemy
            var result = _homeController.AddProject(addProjectViewModel);

            //Assert - trzecia faza unit testu - weryfikacja
            //weryfikujemy ze metoda NewProject faka dla repository byla uruchomiona z paramterem ktory jest rowny zmiennej addProjectViewModel. I zostala uruchomiona tylko raz.
            A.CallTo(() =>
                _repository.NewProject(A<AddProjectViewModel>.That.Matches(a =>
                a == addProjectViewModel))).MustHaveHappened(Repeated.Exactly.Once);

            //weryfikujemy ze metoda AllProjects faka dla repository byla uruchomiona. I zostala uruchomiona tylko raz.
            A.CallTo(() =>
                _repository.AllProjects()).MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            //castujemy result do obiektu zwracanego przez controllera gdy on ma return View(...)
            var viewResult = (ViewResult)result;
            //upewniamy sie ze metoda zwraca poprawny widok
            Assert.AreEqual("ProjectsGrid", viewResult.ViewName);
            //i popranwy model
            Assert.AreEqual(projectList, viewResult.Model);
        }

        [Test]
        public void ProjectsGridTest()
        {
            //Arrange
            List<ProjectsGridViewModel> projectList = new List<ProjectsGridViewModel>
            {
                new ProjectsGridViewModel
                {
                    Title = "first unit test project title",
                    Description = "First unit test project description"
                }
            };
            A.CallTo(() =>
                _repository.AllProjects()).Returns(projectList);
            
            //ACT
            var result = _homeController.ProjectsGrid();

            //Assert

            A.CallTo(() =>
               _repository.AllProjects()).MustHaveHappened(Repeated.Exactly.Once);

           // Assert.NotNull(result);
            var viewResult = (ViewResult)result;
            Assert.AreEqual("ProjectsGrid", viewResult.ViewName);
        }

        [Test]
        public void ErrorTest()
        {
            _homeController.ControllerContext.HttpContext = new DefaultHttpContext();
           
            //ACT
            var result = _homeController.Error();

            //Assert
            A.CallTo(() =>
              _repository.ErrorsLog(A<IExceptionHandlerPathFeature>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            //Assert.NotNull(1);
        }
    }
}