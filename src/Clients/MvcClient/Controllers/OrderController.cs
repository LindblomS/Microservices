namespace mvcclient.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public OrdersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory ?? throw new System.ArgumentNullException(nameof(clientFactory));
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            using (var client = _clientFactory.CreateClient())
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var token = await HttpContext.GetTokenAsync("access_token");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:5001/api/order/" + id);
            }

            return View();
        }
    }
}
