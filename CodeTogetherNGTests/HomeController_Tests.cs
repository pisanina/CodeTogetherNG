using CodeTogetherNG.Controllers;
using CodeTogetherNG.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace CodeTogetherNGTests
{
    [TestFixture]
    public class HomeController_Tests
    {
        //deklarujemy controlera ktorego bedziemy testowac
        private HomeController _homeController;

        //deklaracje dependencies controllera
        private IRepository _repository;

        [SetUp]
        public void CreateControllerForTests()
        {
            //podstawiamy faki pod dependencies
            _repository = A.Fake<IRepository>();

            //tworzymy controlera do trstow z fakami
            _homeController = new HomeController(_repository);
        }

        [Test]
        public void ErrorTest()
        {
            _homeController.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = _homeController.Error();

            A.CallTo(() =>
              _repository.ErrorsLog(A<IExceptionHandlerPathFeature>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}