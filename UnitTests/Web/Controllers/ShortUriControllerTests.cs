using Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Controllers;
using UrlShortener.Models;

namespace UnitTests.Web.Controllers
{
    public class ShortUriControllerTests
    {
        [Test]
        public void Create_WithValidInput_CreatesANewShortUri()
        {
            var shortUriCreatorService = Substitute.For<IShortUriCreator>();
            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), shortUriCreatorService);
            var request = new ShortUriCreationRequest { Uri = new Uri("https://example.com") };

            controller.Create(request);

            shortUriCreatorService.Received().Create(Arg.Is<Uri>(x => x.ToString().Equals(request.Uri.ToString())));
        }
    }
}