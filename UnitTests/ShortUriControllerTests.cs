using Domain.Data;
using Domain.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core;
using UrlShortener.Controllers;
using UrlShortener.Models;

namespace UnitTests
{
    public class ShortUriControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Create_WithValidInput_AddsNewShortUriToDb()
        {
            var repository = Substitute.For<IRepository<ShortUri>>();
            var controller = new ShortUriController(Substitute.For<ILogger<ShortUriController>>(), repository);
            var request = new ShortUriCreationRequest { Uri = new Uri("https://example.com") };

            controller.Create(request);

            repository.Received().Add(Arg.Is<ShortUri>(x => x.Destination.ToString().Equals(request.Uri.ToString())));
        }
    }
}