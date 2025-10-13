using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;

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
            try
            {
                var datos = _context.Producto.ToList();
                return View(datos);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        // GET: Producto/AltaProducto
        public IActionResult AltaProducto()
        {
            try
            {
                ViewData["Tipos"] = GetTipoSelectList();

                return View();
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AltaProducto(Producto producto)
        {
            try
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();

                /// Revisar
                producto.codigo = producto.id_producto.ToString();
                await _context.SaveChangesAsync();
                ///

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }
        private List<SelectListItem> GetTipoSelectList()
        {
            return _context.Producto
           .Select(x => x.tipo)
           .Where(t => t != null && t != "")
           .Distinct()
           .OrderBy(t => t)
           .Select(t => new SelectListItem
           {
               Value = t,
               Text = t
           })
           .ToList();
        }
    }
}
