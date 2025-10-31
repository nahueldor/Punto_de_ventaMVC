using Microsoft.AspNetCore.Mvc;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;
using Punto_de_ventaMVC.Views.ViewsModels;
using static Punto_de_ventaMVC.Views.ViewsModels.DatailsVM;

namespace Punto_de_ventaMVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/venta
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var venta = _context.Venta
                    .Select(p => new
                    {
                        p.id_factura,
                        p.numero,
                        p.cliente,
                        p.usuario,
                        p.fecha_facturacion,
                        p.total,
                        p.subtotal,
                        p.isv,
                        p.descuento
                    })
                    .ToList();

                return Ok(venta); // devuelve JSON automáticamente
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        // GET api/venta/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var venta = _context.Venta.Find(id);

                if (venta == null)
                {
                    return NotFound($"Venta {id} no encontrada");
                }

                return Ok(venta);
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        // GET api/venta/Detalle/5
        [HttpGet("Detalle/{id}")]
        public IActionResult DetalleById(int id)
        {
            try
            {
                var venta = _context.Venta
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

                venta.productosDetalle = new List<ProductoDetalle>(
                    aux.Select(a => new ProductoDetalle()).ToList()
                );

                foreach (var (item, index) in aux.Select((item, index) => (item, index)))
                {
                    venta.productosDetalle[index].detalle = item;
                    venta.productosDetalle[index].producto = _context.Producto
                        .FirstOrDefault(x => x.id_producto == item.producto);
                }

                if (venta == null)
                {
                    return NotFound($"Venta {id} no encontrada");
                }

                return Ok(venta);
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CrearVenta([FromBody] Venta venta)
        {
            try
            {
                if (venta != null)
                {

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    _context.Venta.Add(venta);
                    _context.SaveChanges();

                    return CreatedAtAction(nameof(GetById), new { id = venta.id_factura }, venta);
                }

                return NotFound($"La Venta no se pudo crear");
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Venta model)
        {
            try
            {
                if (model != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var venta = _context.Venta.FirstOrDefault(x => x.id_factura == id);

                    if (venta != null)
                    {
                        venta.id_factura = id;
                        venta.numero = model.numero;
                        venta.cliente = model.cliente;
                        venta.usuario = model.usuario;
                        venta.fecha_facturacion = model.fecha_facturacion;
                        venta.total = model.total;
                        venta.subtotal = model.subtotal;
                        venta.isv = model.isv;
                        venta.descuento = model.descuento;

                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound($"Venta {id} no encontrada");
                    }

                    return Ok(venta);
                }

                return NotFound($"La venta no se pudo editar");
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var venta = _context.Venta.FirstOrDefault(x => x.id_factura == id);

                if (venta != null)
                {
                    _context.Venta.Remove(venta);
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound($"Venta {id} no encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }
    }
}
