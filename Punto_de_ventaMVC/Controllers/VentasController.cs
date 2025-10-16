using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;

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
                Venta = new Venta
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

        public async Task<IActionResult> Create(FacturaViewModel model)
        {
            if (ModelState.IsValid)
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
            model.Clientes = _context.Cliente
                .Select(c => new SelectListItem { Value = c.id_cliente.ToString(), Text = c.nombre })
                .ToList();
            model.Usuarios = _context.Usuario
                .Select(u => new SelectListItem { Value = u.id_usuario.ToString(), Text = u.nombre })
                .ToList();
            model.Productos = _context.Producto
                .Select(p => new SelectListItem { Value = p.id_producto.ToString(), Text = p.nombre })
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
