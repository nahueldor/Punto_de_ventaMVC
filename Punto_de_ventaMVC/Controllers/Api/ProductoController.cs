using Microsoft.AspNetCore.Mvc;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/producto
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var producto = _context.Producto
                    .Select(p => new
                    {
                        p.id_producto,
                        p.nombre,
                        p.precio_compra,
                        p.precio_venta,
                        p.codigo,
                        p.minimo,
                        p.maximo,
                        p.tipo
                    })
                    .ToList();

                return Ok(producto); // devuelve JSON automáticamente
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        // GET api/producto/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var producto = _context.Producto.Find(id);

                if (producto == null)
                {
                    return NotFound($"Producto {id} no encontrado");
                }

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CrearProducto([FromBody] Producto producto)
        {
            try
            {
                if (producto != null)
                {

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    _context.Producto.Add(producto);
                    _context.SaveChanges();

                    /// Revisar - es redundante 'Codigo' con 'id_Producto'
                    producto.codigo = producto.id_producto.ToString();
                    _context.SaveChangesAsync();
                    ///

                    return CreatedAtAction(nameof(GetById), new { id = producto.id_producto }, producto);
                }

                return NotFound($"El Producto no se pudo crear");
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Producto model)
        {
            try
            {
                if (model != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var producto = _context.Producto.FirstOrDefault(x => x.id_producto == id);

                    if (producto != null)
                    {
                        producto.id_producto = id;
                        producto.nombre = model.nombre;
                        producto.precio_compra = model.precio_compra;
                        producto.precio_venta = model.precio_venta;
                        producto.codigo = id.ToString();
                        producto.minimo = model.minimo;
                        producto.maximo = model.maximo;
                        producto.tipo = model.tipo;

                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound($"Producto {id} no encontrado");
                    }

                    return Ok(producto);
                }

                return NotFound($"El Producto no se pudo editar");
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
                var producto = _context.Producto.FirstOrDefault(x => x.id_producto == id);

                if (producto != null)
                {
                    _context.Producto.Remove(producto);
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound($"Producto {id} no encontrado");
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
