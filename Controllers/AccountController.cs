using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice1.Models;
using practice1.Models.ViewModels;
using practice1.Data;
using Microsoft.AspNetCore.Http; // Added for session support

namespace practice1.Controllers
{
    /// <summary>
    /// Controller responsible for user authentication and account management
    /// </summary>
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the login page
        /// </summary>
        /// <returns>Login view</returns>
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Processes the login form submission
        /// </summary>
        /// <param name="model">Login form data</param>
        /// <returns>Redirects to home page on success, or returns to login form with errors</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find the user by email and include their roles
            var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

            // Verify user exists and password is correct using BCrypt
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // Create claims for the user's identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            // Add role claims for authorization
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            // Create the claims identity and principal
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Set authentication properties (remember me, expiration)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            // Sign in the user with cookie authentication
            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);
            
            // Store username in session for easy access in views
            HttpContext.Session.SetString("Username", user.Fullname);
            
            TempData["SuccessMessage"] = "Login successful! Welcome, " + user.Fullname;
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays the registration page
        /// </summary>
        /// <returns>Register view</returns>
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Processes the registration form submission
        /// </summary>
        /// <param name="model">Registration form data</param>
        /// <returns>Redirects to login page on success, or returns to registration form with errors</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email already in use");
                return View(model);
            }

            // Create new user from view model with secure password hashing
            var user = new User
            {
                Email = model.Email,
                Fullname = model.Fullname,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password), // Hash password for security
                CreatedDate = DateTime.Now,
                Status = 1,
                IsActive = true,
                IsLocked = false,
                IsDeleted = false
            };

            // Add and save new user to get generated UserId
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Generates UserId

            // Check if User role exists, create it if it doesn't
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (defaultRole == null)
            {
                defaultRole = new Role("User");
                _context.Roles.Add(defaultRole);
                await _context.SaveChangesAsync();
            }

            // Add user-role relationship to assign the User role
            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = defaultRole.Id
            };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync(); // Save user-role link

            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction(nameof(Login));
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        /// <returns>Redirects to home page</returns>
        [Authorize] // Only authenticated users can log out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Use the correct authentication scheme
            await HttpContext.SignOutAsync("CookieAuth");
            
            // Clear the session
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays the access denied page when a user tries to access a restricted resource
        /// </summary>
        /// <returns>Access denied view</returns>
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}