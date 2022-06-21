namespace Shopping.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Shopping.WebApp.Services;

    public class BasketController : Controller
    {
        readonly IBasketService service;

        public BasketController(IBasketService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteAsync()
        {
            var buyerId = Guid.NewGuid();
            await service.DeleteAsync(buyerId);
            return RedirectToAction("Index", "Home");
        }
    }

}
