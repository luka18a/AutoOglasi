using AutoOglasi.Models;
using AutoOglasi.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoOglasi.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            if (username.Length < 3)
            {
                ViewBag.Error = "Korisničko ime mora imati najmanje 3 karaktera!";
                return View();
            }

            if (password.Length < 6)
            {
                ViewBag.Error = "Lozinka mora imati najmanje 6 karaktera!";
                return View();
            }

            if (_userRepository.GetByUsername(username) != null)
            {
                ViewBag.Error = "Korisničko ime već postoji!";
                return View();
            }

            if (_userRepository.GetByEmail(email) != null)
            {
                ViewBag.Error = "Email već postoji!";
                return View();
            }

            var user = new User
            {
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _userRepository.Create(user);
            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewBag.Error = "Pogrešno korisničko ime ili lozinka!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id!)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}