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

        [HttpGet]
        public ActionResult AltaVenta()
        {
            ViewData["Clientes"] = GetClineteSelectList();
            ViewData["Usuarios"] = GetUsuarioSelectList();
            ViewData["Producto"] = _context.Producto
                    .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
                    .OrderBy(x => x.Text)
                    .ToList();

            return View();
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AltaVenta(FacturaVM model)
        {

            if (ModelState.IsValid && model.Detalles.Any())
            {
                // Guardar la venta
                _context.Venta.Add(model.Venta);
                await _context.SaveChangesAsync();

                // Guardar los detalles
                foreach (var detalle in model.Detalles)
                {
                    detalle.factura = model.Venta.id_factura;
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


        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_factura,numero,cliente,usuario,fecha_facturacion,total,subtotal,isv,descuento")] Venta venta)
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

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
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
             .FirstOrDefault(x => x.id_factura == id);

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

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta != null)
            {
                _context.Venta.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.id_factura == id);
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
