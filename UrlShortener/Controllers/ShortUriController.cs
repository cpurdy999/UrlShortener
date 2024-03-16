using Domain.Data;
using Domain.Data.Models;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class ShortUriController : Controller
    {
        private readonly ILogger<ShortUriController> _logger;
        private readonly IRepository<ShortUri> _repository;

        public ShortUriController(ILogger<ShortUriController> logger, IRepository<ShortUri> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Create(ShortUriCreationRequest request)
        {
            _repository.Add(new ShortUri("", request.Uri));

            return View();
        }
    }
}
