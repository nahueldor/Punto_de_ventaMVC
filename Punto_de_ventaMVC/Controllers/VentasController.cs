using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;
using Punto_de_ventaMVC.Views.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Punto_de_ventaMVC.Views.ViewsModels.DatailsVM;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Punto_de_ventaMVC.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public IActionResult Index()
        {
            try
            {
                var datos = _context.Venta
                 .Join(_context.Cliente,
                       v => v.cliente,
                       c => c.id_cliente,
                       (v, c) => new
                       {
                           v.id_factura,
                           v.numero,
                           clienteNombre = $"{c.nombre} {c.apellido}",
                           usuarioId = v.usuario,
                           v.fecha_facturacion,
                           v.total,
                           v.subtotal,
                           v.isv,
                           v.descuento
                       })
                 .Join(_context.Usuario,
                       vc => vc.usuarioId,
                       u => u.id_usuario,
                       (vc, u) => new VentaVM
                       {
                           id_factura = vc.id_factura,
                           numero = vc.numero,
                           cliente = vc.clienteNombre,
                           usuario = u.nombre,
                           fecha = vc.fecha_facturacion,
                           total = vc.total,
                           subtotal = vc.subtotal,
                           isv = vc.isv,
                           descuento = vc.descuento
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

        [HttpGet]
        public ActionResult AltaVenta()
        {
            try
            {
                ViewData["Clientes"] = GetClineteSelectList();
                ViewData["Usuarios"] = GetUsuarioSelectList();
                ViewData["Producto"] = _context.Producto
                        .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
                        .OrderBy(x => x.Text)
                        .ToList();

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
        public async Task<IActionResult> AltaVenta(FacturaVM model)
        {
            try
            {
                if (ModelState.IsValid && model.Detalles.Any())
                {
                    // Guardar la venta
                    _context.Venta.Add(model.venta);
                    await _context.SaveChangesAsync();

                    // Guardar los detalles
                    foreach (var detalle in model.Detalles)
                    {
                        detalle.factura = model.venta.id_factura;
                        _context.FacturaDetalle.Add(detalle);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                // Si hay errores, recargar listas
                ViewData["Clientes"] = GetClineteSelectList();
                ViewData["Usuarios"] = GetUsuarioSelectList();
                ViewData["Producto"] = _context.Producto
                        .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
                        .OrderBy(x => x.Text)
                        .ToList();


                return View(model);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }


        // GET: Ventas/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ViewData["Clientes"] = GetClineteSelectList();
                ViewData["Usuarios"] = GetUsuarioSelectList();
                ViewData["Producto"] = _context.Producto
                       .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
                       .OrderBy(x => x.Text)
                       .ToList();

                var venta = await _context.Venta.FindAsync(id);
                if (venta == null)
                {
                    return NotFound();
                }

                var model = new FacturaEditVM()
                {
                    venta = venta
                };

                var aux = _context.FacturaDetalle.Where(x => x.factura == id).ToList();

                foreach (var (item, index) in aux.Select((item, index) => (item, index)))
                {
                    model.Detalles.Add(item);
                    model.Productos.Add(_context.Producto
                        .FirstOrDefault(x => x.id_producto == item.producto) ?? new Producto());
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(Prefix = "Model")] FacturaEditVM model)
        {
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"{key} = {Request.Form[key]}");
            }

            var venta = model.venta;
            try
            {
                if (id != venta.id_factura)
                {
                    return NotFound();
                }
                //venta.Cliente = _context.Cliente.FirstOrDefault(x => x.id_cliente == venta.cliente)
                //          ?? new Cliente();
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(venta);
                        await _context.SaveChangesAsync();

                        // Eliminar los detalles viejos
                        var detallesViejos = _context.FacturaDetalle
                            .Where(d => d.factura == model.venta.id_factura);
                        _context.FacturaDetalle.RemoveRange(detallesViejos);

                        // Agregar los nuevos
                        foreach (var d in model.Detalles)
                        {
                            d.factura = model.venta.id_factura;
                            _context.FacturaDetalle.Add(d);
                        }

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!VentaExists(venta.id_factura))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(venta);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        // GET: Ventas/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var venta = await _context.Venta
                    .FirstOrDefaultAsync(m => m.id_factura == id);
                if (venta == null)
                {
                    return NotFound();
                }

                return View(venta);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        // GET: Ventas/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var datos = _context.Venta
                 .Join(_context.Cliente,
                       v => v.cliente,
                       c => c.id_cliente,
                       (v, c) => new
                       {
                           v.id_factura,
                           v.numero,
                           clienteNombre = $"{c.nombre} {c.apellido}",
                           clienteId = c.id_cliente,
                           usuarioId = v.usuario,
                           v.fecha_facturacion,
                           v.total,
                           v.subtotal,
                           v.isv,
                           v.descuento
                       })
                 .Join(_context.Usuario,
                       vc => vc.usuarioId,
                       u => u.id_usuario,
                       (vc, u) => new DatailsVM
                       {
                           venta = new Venta
                           {
                               id_factura = vc.id_factura,
                               numero = vc.numero,
                               cliente = vc.clienteId,
                               usuario = u.id_usuario,
                               fecha_facturacion = vc.fecha_facturacion,
                               total = vc.total,
                               subtotal = vc.subtotal,
                               isv = vc.isv,
                               descuento = vc.descuento
                           },
                           Cliente = vc.clienteNombre,
                           Usuario = u.nombre,
                       })
                 .FirstOrDefault(x => x.venta.id_factura == id);

                var aux = _context.FacturaDetalle.Where(x => x.factura == id).ToList();

                datos.productosDetalle = new List<ProductoDetalle>(
                    aux.Select(a => new ProductoDetalle()).ToList()
                );

                foreach (var (item, index) in aux.Select((item, index) => (item, index)))
                {
                    datos.productosDetalle[index].detalle = item;
                    datos.productosDetalle[index].producto = _context.Producto
                        .FirstOrDefault(x => x.id_producto == item.producto);
                }

                if (datos == null)
                {
                    return NotFound();
                }

                return View(datos);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var venta = await _context.Venta.FindAsync(id);
                if (venta != null)
                {
                    _context.Venta.Remove(venta);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }
        private bool VentaExists(int id)
        {
            try
            {
                return _context.Venta.Any(e => e.id_factura == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);

                return false;
            }
        }

        [HttpGet]
        public IActionResult GetPrecioProducto(int id)
        {
            try
            {
                var producto = _context.Producto
                .Where(p => p.id_producto == id)
                .Select(p => new { p.precio_venta })
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
        private List<SelectListItem> GetClineteSelectList()
        {
            var cliente = _context.Cliente
            .OrderBy(x => x.nombre)
            .Select(p => new SelectListItem
            {
                Value = p.id_cliente.ToString(),
                Text = $"{p.nombre} {p.apellido}"
            }).ToList();

            return cliente;
        }

        private List<SelectListItem> GetUsuarioSelectList()
        {
            var usuario = _context.Usuario
            .OrderBy(x => x.nombre)
            .Select(p => new SelectListItem
            {
                Value = p.id_usuario.ToString(),
                Text = p.nombre
            }).ToList();

            return usuario;
        }
    }
}
