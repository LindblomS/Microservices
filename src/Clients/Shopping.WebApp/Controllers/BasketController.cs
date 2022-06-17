namespace Shopping.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BasketController : Controller
    { 
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[Route("{productId}")]
        //public IActionResult Delete(string productId)
        //{

        //}
    }

}
