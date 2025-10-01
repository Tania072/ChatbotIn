using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Prueba2.Data;
using Prueba2.Models;
using System.Security.Claims;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace Prueba2.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Auth/Login - ✅ MÉTODO FALTANTE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Buscar usuario por email
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (usuario != null && BCrypt.Net.BCrypt.Verify(model.Password, usuario.PasswordHash))
                {
                    // Iniciar sesión
                    await SignInUser(usuario);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError("", "Email o contraseña incorrectos");
            }

            return View(model);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el email ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Este email ya está registrado.");
                    return View(model);
                }

                // 🔥 CORRECCIÓN: Todos los usuarios se crean como ADMINISTRADORES
                var usuario = new Usuario
                {
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Telefono = model.Telefono,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    FechaRegistro = DateTime.UtcNow,
                    Rol = "admin" // 👈 TODOS SON ADMINISTRADORES
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Iniciar sesión automáticamente
                await SignInUser(usuario);

                TempData["SuccessMessage"] = "✅ Administrador registrado exitosamente";
                return RedirectToAction("Dashboard", "Home");
            }

            return View(model);
        }

        // GET: /Auth/GoogleLogin
        [HttpGet]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "Auth", new { returnUrl })
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // GET: /Auth/GoogleCallback
        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return RedirectToAction("Login");

            var claims = result.Principal.Claims;
            var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var nombre = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var fotoUrl = claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(googleId))
                return RedirectToAction("Login");

            // Buscar usuario existente
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.GoogleId == googleId || u.Email == email);

            if (usuario == null)
            {
                // Crear nuevo usuario desde Google
                usuario = new Usuario
                {
                    Email = email,
                    Nombre = nombre ?? "Usuario Google",
                    GoogleId = googleId,
                    FotoUrl = fotoUrl ?? "",
                    PasswordHash = "GOOGLE_AUTH",
                    FechaRegistro = DateTime.UtcNow,
                    Rol = "admin" // 👈 TODOS SON ADMINISTRADORES
                };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }

            await SignInUser(usuario);

            return RedirectToAction("Dashboard", "Home");
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("FotoUrl", usuario.FotoUrl ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }

    // ✅ MODELOS PARA LOS FORMULARIOS
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmar contraseña es requerido")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}