using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;
using Punto_de_ventaMVC.Views.ViewsModels;
using System.Text.Json;

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
                          tipo = inventario.tipo,
                          stock_minimo = producto.minimo,
                          stock_maximo = producto.maximo
                      })
                .Select(result => new InventarioVM
                {
                    id_inventario = result.id_inventario,
                    codigo = result.codigo,
                    producto = result.producto,
                    cantidad = result.cantidad,
                    fecha_inventario = result.fecha_inventario,
                    tipo = result.tipo,
                    stock_minimo = result.stock_minimo,
                    stock_maximo = result.stock_maximo
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
                var model = new AltaModificacionInventarioVM
                {
                    ProductosListItem = GetProductoSelectList() ?? throw new ArgumentException("No se encontro la tabla producto.")
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
                    return RedirectToAction(nameof(Index));
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

        public IActionResult ModificarInventario(int id)
        {
            try
            {
                var model = new AltaModificacionInventarioVM
                {
                    ProductosListItem = GetProductoSelectList() ?? throw new ArgumentException("No se encontro la tabla producto."),
                    inventario = _context.Inventario.FirstOrDefault(x => x.id_inventario == id) ?? throw new ArgumentException($"Inventario {id} no encontrado.")
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarInventario(Inventario inventario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Guardar la modificacion del inventario en la base de datos
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> BajarInventario(int id)
        {
            try
            {
                var inventario = await _context.Inventario.FirstOrDefaultAsync(x => x.id_inventario == id);

                if (inventario == null)
                {
                    throw new ArgumentException($"Inventario {id} no encontrado.");
                }

                _context.Inventario.Remove(inventario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error");
            }
        }


        [HttpGet]
        public IActionResult GetTipoProducto(int id)
        {
            try
            {
                var producto = _context.Producto
                .Where(p => p.id_producto == id)
                .Select(p => new { p.tipo, p.minimo, p.maximo })
                .FirstOrDefault();

                if (producto == null)
                {
                    throw new ArgumentException($"No se encontro el producto {id}.");
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

        private List<SelectListItem> GetProductoSelectList()
        {
            var producto = _context.Producto
            .OrderBy(x => x.nombre)
            .Select(p => new SelectListItem
            {
                Value = p.id_producto.ToString(),
                Text = p.nombre
            }).ToList();

            return producto;
        }
    }
}
