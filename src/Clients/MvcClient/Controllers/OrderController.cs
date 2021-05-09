namespace mvcclient.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Orders()
        {
            return View();
        }
    }
}
