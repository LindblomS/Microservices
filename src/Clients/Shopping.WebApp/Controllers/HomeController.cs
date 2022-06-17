namespace Shopping.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Shopping.WebApp.Models;
    using Shopping.WebApp.Services;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        readonly CatalogService service;

        public HomeController(CatalogService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await service.GetAsync();
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}