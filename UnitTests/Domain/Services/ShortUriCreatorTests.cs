using Domain.Data;
using Domain.Data.Models;
using Domain.Services;
using NSubstitute;

namespace UnitTests.Domain.Services
{
    public class ShortUriCreatorTests
    {
        private ShortUriCreator shorUriCreator { get; set; }

        [SetUp]
        public void Setup()
        {
            var repository = Substitute.For<IRepository<ShortUri>>();
            shorUriCreator = new ShortUriCreator(repository, new Random());
        }

        [Test]
        public void Create_ReturnsAShortUri_With_CorrectData()
        {
            var inputUri = "https://example.com";

            var result = shorUriCreator.Create(inputUri);

            Assert.IsNotNull(result);
            Assert.AreEqual(inputUri.ToString(), result.Destination.ToString());
        }

        [Test]
        public void Create_ReturnsAShortUri_With_8CharactersInAccessTag()
        {
            var inputUri = "https://example.com";

            var result = shorUriCreator.Create(inputUri);

            Assert.AreEqual(result.AccessTag.Length, 8);
        }

        [Test]
        public void Create_SavesTheShortUriToTheRepository()
        {
            var repository = Substitute.For<IRepository<ShortUri>>();
            var localShortUriCreator = new ShortUriCreator(repository, new Random());

            var inputUri = "https://example.com";

            var result = localShortUriCreator.Create(inputUri);

            repository.Received().Add(Arg.Is(result));
            repository.Received().SaveChanges();
        }

    }
}