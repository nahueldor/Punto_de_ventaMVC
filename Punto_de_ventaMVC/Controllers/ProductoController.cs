using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;
using Punto_de_ventaMVC.Views.ViewsModels;
using System.Text.Json;

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
                decimal cotizacion_dolar = getObtenerDolarVentaAsync();

                var datos = _context.Producto.Select(result => new ProductoVM
                {
                    id_producto = result.id_producto,
                    nombre = result.nombre,
                    precio_compra_peso = result.precio_compra,
                    precio_compra_dolar = Math.Round(result.precio_compra / cotizacion_dolar ?? 0, 2),
                    precio_venta_peso = result.precio_venta,
                    precio_venta_dolar = Math.Round(result.precio_venta / cotizacion_dolar ?? 0, 2),
                    codigo = result.codigo,
                    minimo = result.minimo,
                    maximo = result.maximo,
                    tipo = result.tipo
                }).ToList();

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
                ViewData["Tipos"] = GetTipoSelectList() ?? throw new ArgumentException("No se encontro la tabla producto.");

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

                if (ModelState.IsValid)
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();

                    /// Revisar - es redundante 'Codigo' con 'id_Producto'
                    producto.codigo = producto.id_producto.ToString();
                    await _context.SaveChangesAsync();
                    ///

                    return RedirectToAction(nameof(Index));
                }

                ViewData["Tipos"] = GetTipoSelectList() ?? throw new ArgumentException("No se encontro la tabla producto.");
                return View(producto);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        public IActionResult ModificarProducto(int id)
        {
            try
            {

                var producto = _context.Producto.FirstOrDefault(x => x.id_producto == id) ?? throw new ArgumentException($"Producto {id} no encontrado.");

                ViewData["Tipos"] = GetTipoSelectList() ?? throw new ArgumentException("No se encontro la tabla producto.");

                return View(producto);
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
        public async Task<IActionResult> ModificarProducto(Producto producto)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                ViewData["Tipos"] = GetTipoSelectList() ?? throw new ArgumentException("No se encontro la tabla producto.");
                return View(producto);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        [HttpGet]
        public async Task<IActionResult> BajarProducto(int id)
        {
            try
            {
                var producto = await _context.Producto.FirstOrDefaultAsync(x => x.id_producto == id);

                if (producto == null)
                {
                    throw new ArgumentException($"Producto {id} no encontrado.");
                }

                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error");
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

        private static int getObtenerDolarVentaAsync()
        {
            return Task.Run(async () =>
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync("https://dolarapi.com/v1/dolares/oficial");

                using var doc = JsonDocument.Parse(json);
                var venta = doc.RootElement.GetProperty("venta").GetDecimal();

                return (int)Math.Round(venta);
            }).Result; // Espera el resultado del Task sin necesidad de async externo
        }
    }
}
