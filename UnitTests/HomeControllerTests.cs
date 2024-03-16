using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Controllers;

namespace UnitTests
{
    public class HomeControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Index_Returns_a_ViewResult()
        {
            var controller = new HomeController(Substitute.For<ILogger<HomeController>>());

            var result = controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}