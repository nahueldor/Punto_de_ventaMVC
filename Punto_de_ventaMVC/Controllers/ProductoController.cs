using Microsoft.AspNetCore.Mvc;
using Punto_de_ventaMVC.Data;

namespace Punto_de_ventaMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var datos = _context.Producto.ToList();
            return View(datos);
        }
    }
}
