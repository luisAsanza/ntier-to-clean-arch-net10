using System.Security.Claims;
using CleanCRUDSolution.Application.Features.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanCRUDSolution.Web.Models.Account;
using CleanCRUDSolution.Web.Extensions;
using Microsoft.Extensions.Options;
using CleanCRUDSolution.Web.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CleanCRUDSolution.Web.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly AuthCookieOptions _cookieOptions;

        public AccountController(IIdentityService identityService, IOptions<AuthCookieOptions> cookieOptions)
        {
            _identityService = identityService;
            _cookieOptions = cookieOptions.Value;
        }

        [HttpGet("Login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var result = await _identityService.CheckPasswordAsync(model.Email, model.Password);
            if (!result.IsSuccess || result.Value == false)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email)
            };

            var identity = new ClaimsIdentity(
                claims: claims,
                authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { 
                    IsPersistent = true,
                    Items = { { "AbsoluteExpirationTicks", DateTime.UtcNow.AddHours(_cookieOptions.AbsoluteExpirationHours).Ticks.ToString() } }
                    });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Persons");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _identityService.CreateUserAsync(model.Email, model.Password);
            if (!result.IsSuccess)
            {
                ModelState.AddApplicationErrors(result.Errors);
                return View(model);
            }

            var userId = result.Value;
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, model.Email),
                new(ClaimTypes.Email, model.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                principal,
                new AuthenticationProperties { 
                    IsPersistent = true,
                    Items = { { "AbsoluteExpirationTicks", DateTime.UtcNow.AddHours(_cookieOptions.AbsoluteExpirationHours).Ticks.ToString() } }
                    });

            return RedirectToAction("Index", "Persons");
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
