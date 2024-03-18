using Domain.Data.Models;
using Domain.Data;
using Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Controllers;
using UrlShortener.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.Extensions;

namespace UnitTests.Web.Controllers
{
    public class ShortUriControllerTests
    {
        private IShortUriCreator defaultShortUriCreator;

        [SetUp]
        public void Setup()
        {
            defaultShortUriCreator = Substitute.For<IShortUriCreator>();
            defaultShortUriCreator.Create("").ReturnsForAnyArgs(x => new ShortUri("AAAAAA", x.ArgAt<string>(0)));
        }

        [Test]
        public void Index_WithExistingAccessTag_ReturnsARedirectResultToCorrectUri()
        {
            string existingAccessTag = "AAAA";
            string correctUriResult = "https://example.com";

            var repository = Substitute.For<IRepository<ShortUri>>();

            var dbList = new List<ShortUri>
            {
                new ShortUri(existingAccessTag, correctUriResult)
            };

            repository.Items.ReturnsForAnyArgs(dbList.AsQueryable());

            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, defaultShortUriCreator);

            var result = controller.Index(accessTag: existingAccessTag) as RedirectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url.ToString(), Is.EqualTo(correctUriResult.ToString()));
        }

        [Test]
        public void Index_WithNonExistentAccessTag_ReturnsAView()
        {
            string existingAccessTag = "AAAA";
            string existingUriResult = "https://example.com";

            string nonExistentAccessTag = "BBBB";

            var shortUriCreatorService = Substitute.For<IShortUriCreator>();
            var repository = Substitute.For<IRepository<ShortUri>>();

            var dbList = new List<ShortUri>
            {
                new ShortUri(existingAccessTag, existingUriResult)
            };

            repository.Items.ReturnsForAnyArgs(dbList.AsQueryable());

            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, shortUriCreatorService);

            var result = controller.Index(accessTag: nonExistentAccessTag) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Create_Get_ReturnsAView_WithANewCreationRequestModel()
        {
            var repository = Substitute.For<IRepository<ShortUri>>();
            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, defaultShortUriCreator);

            var result = controller.Create() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<ShortUriCreationRequest>());

            var model = result.Model as ShortUriCreationRequest;

            Assert.That(model.Uri, Is.Null);
        }

        [Test]
        public void Create_Post_WithValidInput_CreatesANewShortUri()
        {
            var repository = Substitute.For<IRepository<ShortUri>>();
            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, defaultShortUriCreator);
            var request = new ShortUriCreationRequest { Uri = "https://example.com" };

            controller.Create(request);

            defaultShortUriCreator.Received().Create(Arg.Is<string>(x => x.Equals(request.Uri)));
        }

        [Test]
        public void Create_Post_WithValidInput_Redirects_To_Success_Page()
        {
            var repository = Substitute.For<IRepository<ShortUri>>();
            var shortUriCreatorService = Substitute.For<IShortUriCreator>();

            var newAccessTag = "ABCDEF";
            shortUriCreatorService.Create("").ReturnsForAnyArgs(x => new ShortUri(newAccessTag, x.ArgAt<string>(0)));

            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, shortUriCreatorService);
            var request = new ShortUriCreationRequest { Uri = "https://example.com" };

            var result = controller.Create(request) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ControllerName, Is.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(ShortUriController.Success)));

            Assert.That(result.RouteValues, Contains.Key("accessTag"));
            Assert.That(result.RouteValues["accessTag"], Is.EqualTo(newAccessTag));
        }

        [Test]
        [Ignore("Requires further investigation for mocking UrlHelper")]
        public void Success_ReturnsAView()
        {
            var shortUriCreatorService = Substitute.For<IShortUriCreator>();
            var repository = Substitute.For<IRepository<ShortUri>>();

            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, shortUriCreatorService);

            var result = controller.Success(accessTag: "AAAAAA") as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Ignore("Requires further investigation for mocking UrlHelper")]
        public void Success_SetsTheNewLinkForTheView()
        {
            //TODO: Create mock UrlHelper to allow this to be unit tested.
            //Base NSub config doesn't work due to ActionLink being an extension method, looks like it will need complex setup with ActionContext

            var shortUriCreatorService = Substitute.For<IShortUriCreator>();
            var repository = Substitute.For<IRepository<ShortUri>>();

            var urlHelper = Substitute.For<IUrlHelper>();

            urlHelper.Configure().ActionLink(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>()).ReturnsForAnyArgs(x =>
            {
                return $"shorturl.com/{(x.Args()[2] as Dictionary<string, string>)["accessTag"]}";
            });

            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository, shortUriCreatorService);
            controller.Url = urlHelper;

            var accessTag = "ABCDEF";

            var result = controller.Success(accessTag: accessTag);

            Assert.That(controller.ViewBag.CreatedLink, Is.EqualTo("shorturl.com/ABCDEF"));
        }
    }
}