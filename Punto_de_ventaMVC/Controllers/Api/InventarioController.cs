using Microsoft.AspNetCore.Mvc;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;
using System;

namespace Punto_de_ventaMVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/inventario
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var invetario = _context.Inventario
                    .Select(p => new
                    {
                        p.id_inventario,
                        p.producto,
                        p.cantidad,
                        p.fecha_inventario,
                        p.tipo
                    })
                    .ToList();

                return Ok(invetario); // devuelve JSON automáticamente
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        // GET api/inventario/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var inventario = _context.Inventario.Find(id);

                if (inventario == null)
                {
                    return NotFound($"Inventario {id} no encontrado");
                }

                return Ok(inventario);
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CrearIventario([FromBody] Inventario inventario)
        {
            try
            {
                if (inventario != null)
                {
                    inventario.fecha_inventario = DateTime.Now;

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    _context.Inventario.Add(inventario);
                    _context.SaveChanges();

                    return CreatedAtAction(nameof(GetById), new { id = inventario.id_inventario }, inventario);
                }

                return NotFound($"El Inventario no se pudo crear");
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Inventario model)
        {
            try
            {
                if (model != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var inventario = _context.Inventario.FirstOrDefault(x => x.id_inventario == id);

                    if (inventario != null)
                    {
                        inventario.id_inventario = id;
                        inventario.producto = model.producto;
                        inventario.cantidad = model.cantidad;
                        inventario.fecha_inventario = model.fecha_inventario;
                        inventario.tipo = model.tipo;

                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound($"Inventario {id} no encontrado");
                    }

                    return Ok(inventario);
                }

                return NotFound($"El Inventario no se pudo editar");
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
                var inventario = _context.Inventario.FirstOrDefault(x => x.id_inventario == id);

                if (inventario != null)
                {
                    _context.Inventario.Remove(inventario);
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound($"Inventario {id} no encontrado");
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
