using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Controllers;

namespace UnitTests.Web.Controllers
{
    public class HomeControllerTests
    {
        [Test]
        public void Index_Returns_a_ViewResult()
        {
            var controller = new HomeController(Substitute.For<ILogger<HomeController>>());

            var result = controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}