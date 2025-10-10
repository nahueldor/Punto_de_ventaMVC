using Microsoft.AspNetCore.Mvc;
using Punto_de_ventaMVC.Data;

namespace Punto_de_ventaMVC.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var datos = _context.Cliente.ToList();
            return View(datos);
        }
    }
}
