using Domain.Data;
using Domain.Data.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class ShortUriController : Controller
    {
        private readonly ILogger<ShortUriController> _logger;
        private readonly IRepository<ShortUri> _repository;
        private IShortUriCreator _shortUriCreator;

        public ShortUriController(ILogger<ShortUriController> logger, IRepository<ShortUri> repository, IShortUriCreator shortUriCreator)
        {
            _logger = logger;
            _shortUriCreator = shortUriCreator;
            _repository = repository;
        }

        public IActionResult Index(string accessTag)
        {
            var shortUri = _repository.Items.FirstOrDefault(x => x.AccessTag == accessTag);

            if (shortUri != null)
            {
                return Redirect(shortUri.Destination);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ShortUriCreationRequest());
        }

        [HttpPost]
        public IActionResult Create(ShortUriCreationRequest request)
        {
            var result = _shortUriCreator.Create(request.Uri);
            return RedirectToAction(nameof(Success), new { accessTag = result.AccessTag });
        }

        public IActionResult Success(string accessTag)
        {
            var action = Url.Action("ShortUri", "Index", new { accessTag });

            var link = Url.ActionLink(nameof(Index), values: new Dictionary<string, string>{ { nameof(accessTag), accessTag } });
            ViewBag.CreatedLink = link;
            return View();
        }
    }
}
