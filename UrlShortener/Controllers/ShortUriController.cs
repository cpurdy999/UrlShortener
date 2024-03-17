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
        private IShortUriCreator _shortUriCreator;

        public ShortUriController(ILogger<ShortUriController> logger, IShortUriCreator shortUriCreator)
        {
            _logger = logger;
            _shortUriCreator = shortUriCreator;
        }

        public IActionResult Create(ShortUriCreationRequest request)
        {
            _shortUriCreator.Create(request.Uri);
            return View();
        }
    }
}
