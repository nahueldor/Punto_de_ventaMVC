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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venta
                .Include(v => v.Cliente)  // Esto hace el join con la tabla clientes
                .ToListAsync());

        }

        // GET: Ventas/Create

        public IActionResult Create()
        {
            var model = new FacturaViewModel
            {
                Venta = new Venta()
                {
                    fecha_facturacion = DateTime.Now
                },
                Clientes = _context.Cliente
                    .Select(c => new SelectListItem { Value = c.id_cliente.ToString(), Text = c.nombre })
                    .ToList(),
                Usuarios = _context.Usuario
                    .Select(u => new SelectListItem { Value = u.id_usuario.ToString(), Text = u.nombre })
                    .ToList(),
                Productos = _context.Producto
                    .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
                    .ToList()
            };

            return View(model);
        }


        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("id_factura,numero,cliente,usuario,fecha_facturacion,total,subtotal,isv,descuento")] Venta venta)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(venta);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(venta);
        //}

        public async Task<IActionResult> Create(FacturaVM model)
        {

            Venta v = new Venta()
            {
                id_factura = model.Venta.id_factura,
                numero = model.Venta.numero,
                cliente = model.Venta.cliente,
                Cliente = _context.Cliente.FirstOrDefault(x => x.id_cliente == model.Venta.cliente)
                      ?? new Cliente(),
                usuario = model.Venta.usuario,
                fecha_facturacion = model.Venta.fecha_facturacion,
                total = model.Venta.total,
                subtotal = model.Venta.subtotal,
                isv = model.Venta.isv,
                descuento = model.Venta.descuento,
                Detalles = null
            };

            List<FacturaDetalle> d = new List<FacturaDetalle>();

            foreach (var fd in model.Detalles)
            {
                FacturaDetalle auxd = new FacturaDetalle()
                {
                    id_descripcion = fd.id_descripcion,
                    Venta = (Venta)v,
                    producto = fd.producto,
                    Producto = _context.Producto.FirstOrDefault(x => x.id_producto == fd.producto)
                           ?? new Producto(),
                    cantidad = fd.cantidad,
                    precio = fd.precio,
                };
            }

            v.Detalles = d.ToArray();

            if (ModelState.IsValid&& !model.Detalles.Any())
            {
                // Guardar la venta
                _context.Venta.Add(v);
                await _context.SaveChangesAsync();

                // Guardar los detalles
                foreach (var detalle in d)
                {
                    detalle.factura = model.Venta.id_factura;
                    _context.FacturaDetalle.Add(detalle);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            FacturaViewModel modelViewModel = new FacturaViewModel()
            {
                Venta = v,
                Detalles = d,
            };

            // Si hay errores, recargar listas
            modelViewModel.Clientes = _context.Cliente
                .Select(c => new SelectListItem { Value = c.id_cliente.ToString(), Text = c.nombre })
                .ToList();
            modelViewModel.Usuarios = _context.Usuario
                .Select(u => new SelectListItem { Value = u.id_usuario.ToString(), Text = u.nombre })
                .ToList();
            modelViewModel.Productos = _context.Producto
                .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
                .ToList();

            return View(modelViewModel);
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

            var venta = await _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.Detalles) // Incluye los ítems de la factura
                .ThenInclude(d => d.Producto) // Esto incluye el nombre del producto
                .FirstOrDefaultAsync(m => m.id_factura == id);

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
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
    }
}
