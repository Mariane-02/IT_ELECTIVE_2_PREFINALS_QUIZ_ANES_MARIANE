using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class AccountController : Controller
{
    private readonly AuthService _auth;

    public AccountController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var key = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (_auth.IsLockedOut(key, out var remaining))
        {
            ModelState.AddModelError(string.Empty,
                $"Too many failed attempts. Please try again in {Math.Ceiling(remaining.TotalMinutes)} minute(s).");
            return View(model);
        }

        if (!_auth.Validate(model.Username, model.Password, key))
        {
            // Same message for both cases so the response does not reveal which field was wrong.
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = false });

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
