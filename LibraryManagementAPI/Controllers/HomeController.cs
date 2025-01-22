using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryManagementAPI.Models;

public class HomeController : Controller
{
    // Action to show the login form
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Action to handle form submission and authentication
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        // This is just a simple hardcoded check for demonstration purposes.
        // In a real app, you'd want to validate the user's credentials using a database.
        if (email == "user@example.com" && password == "password")
        {
            // Create the claims (User Identity)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email)
            };

            // Create the claims identity
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create the authentication properties
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // To keep the user logged in between sessions
            };

            // Sign the user in
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect to the home page or dashboard after successful login
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Return an error message if login fails
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }
    }

    // Action to handle logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}