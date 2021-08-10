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

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AccountController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
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
                var user = new IdentityUser(vm.Username);
                user.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
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

    }
}
