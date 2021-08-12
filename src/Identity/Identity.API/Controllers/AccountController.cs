namespace Identity.API.Controllers
{
    using Identity.API.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System;
    using IdentityServer4.Services;
    using Services.Identity.Infrastructure;
    using System.Net.Http;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using Newtonsoft.Json;

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService,
            IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

                if (result.Succeeded)
                    return Redirect(vm.ReturnUrl);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("request_id", Guid.NewGuid().ToString());
                var password = SHA256.HashData(Encoding.Default.GetBytes(vm.Password));
                var command = new CreateUserCommand(vm.Username, password.ToString(), new List<Claim>(), new List<string>());

                var json = JsonConvert.SerializeObject(command);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("https://localhost:5001/api/user", content);

                if (result.IsSuccessStatusCode)
                {
                    var user = await _userManager.FindByNameAsync(vm.Username);
                    await _signInManager.SignInAsync(user, false);
                    return Redirect(vm.ReturnUrl);
                }

                throw new Exception(result.ToString());
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logOutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(logOutRequest.PostLogoutRedirectUri))
                return RedirectToAction("Index", "Account");

            return Redirect(logOutRequest.PostLogoutRedirectUri);
        }

        public string Index()
        {
            return "Hola";
        }

        private record CreateUserCommand(string username, string passwordHash, IEnumerable<Claim> claims, IEnumerable<string> roles);
        private record Claim(string type, string value);

    }
}
