using Domain.Data.Models;
using Domain.Data;
using Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Controllers;
using UrlShortener.Models;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Web.Controllers
{
    public class ShortUriControllerTests
    {
        [Test]
        public void Index_WithExistingAccessTag_ReturnsARedirectResultToCorrectUri()
        {
            string existingAccessTag = "AAAA";
            string correctUriResult = "https://example.com";

            var shortUriCreatorService = Substitute.For<IShortUriCreator>();
            var repository = Substitute.For<IRepository<ShortUri>>();

            var dbList = new List<ShortUri>
            {
                new ShortUri(existingAccessTag, correctUriResult)
            };

            repository.Items.ReturnsForAnyArgs(dbList.AsQueryable());

            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, shortUriCreatorService);

            var result = controller.Index(accessTag: existingAccessTag) as RedirectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url.ToString(), Is.EqualTo(correctUriResult.ToString()));
        }

        [Test]
        public void Create_WithValidInput_CreatesANewShortUri()
        {
            var shortUriCreatorService = Substitute.For<IShortUriCreator>();
            var repository = Substitute.For<IRepository<ShortUri>>();
            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, shortUriCreatorService);
            var request = new ShortUriCreationRequest { Uri = "https://example.com" };

            controller.Create(request);

            shortUriCreatorService.Received().Create(Arg.Is<string>(x => x.Equals(request.Uri)));
        }
    }
}