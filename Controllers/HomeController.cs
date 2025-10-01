using Microsoft.AspNetCore.Mvc;

namespace ChatbotIn.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        public IActionResult Portada()
        {
            return View();
        }

        public IActionResult Pedidos() { return View(); }
        public IActionResult Productos() { return View(); }
        public IActionResult Reportes() { return View(); }
        public IActionResult Soporte() { return View(); }
    }
}
