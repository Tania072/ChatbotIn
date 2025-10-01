using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba2.Data;
using Prueba2.Models;
using System.Security.Claims;

namespace Prueba2.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Where(p => p.Activo)
                .Include(p => p.Vendedor)
                .ToListAsync();
                
            return View(productos);
        }

        // GET: /Productos/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    producto.VendedorId = userId;
                    producto.FechaCreacion = DateTime.UtcNow;
                    producto.FechaActualizacion = DateTime.UtcNow;

                    _context.Productos.Add(producto);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Producto creado exitosamente!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear producto: {ex.Message}");
                }
            }

            return View(producto);
        }
    }
}