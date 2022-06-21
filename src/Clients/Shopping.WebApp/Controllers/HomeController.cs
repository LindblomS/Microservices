namespace Shopping.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Shopping.WebApp.Models;
    using Shopping.WebApp.Services;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        readonly ICatalogService catalogService;
        readonly IBasketService basketService;

        public HomeController(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await catalogService.GetAsync();
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