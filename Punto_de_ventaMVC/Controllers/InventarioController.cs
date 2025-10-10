using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;
using Punto_de_ventaMVC.Views.ViewsModels;

namespace Punto_de_ventaMVC.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                //var datos = _context.Inventario.ToList();
                var datos = _context.Inventario
                .Join(_context.Producto,
                      inventario => inventario.producto,  // Clave foránea en Inventario
                      producto => producto.id_producto,              // Clave primaria en Producto
                      (inventario, producto) => new
                      {
                          id_inventario = inventario.id_inventario,
                          codigo = producto.id_producto,
                          producto = producto.nombre,
                          cantidad = inventario.cantidad,
                          fecha_inventario = inventario.fecha_inventario,
                          tipo = inventario.tipo
                      })
                .Select(result => new InventarioVM
                {
                    id_inventario = result.id_inventario,
                    codigo = result.codigo,
                    producto = result.producto,
                    cantidad = result.cantidad,
                    fecha_inventario = result.fecha_inventario,
                    tipo = result.tipo
                })
                .ToList();

                return View(datos);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }


        // GET: Inventarios/AltaInventario
        public IActionResult AltaInventario()
        {
            try
            {
                var model = new AltaInventarioVM
                {
                    ProductosListItem = _context.Producto
                .Select(p => new SelectListItem
                {
                    Value = p.id_producto.ToString(),
                    Text = p.nombre
                }).ToList()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        // POST: Inventarios/AltaInventario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AltaInventario(Inventario inventario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Guardar el inventario en la base de datos
                    _context.Add(inventario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(inventario);  // Si hay un error, regresa al formulario
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        [HttpGet]
        public IActionResult GetTipoProducto(int id)
        {
            try
            {
                var producto = _context.Producto
                .Where(p => p.id_producto == id)
                .Select(p => new { p.id_producto, p.nombre, p.tipo })
                .FirstOrDefault();

                if (producto == null)
                {
                    return NotFound();
                }

                return Json(producto);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }
    }
}
