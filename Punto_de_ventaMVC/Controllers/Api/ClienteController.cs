using Microsoft.AspNetCore.Mvc;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/cliente
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var cliente = _context.Cliente
                    .Select(p => new
                    {
                        p.id_cliente,
                        p.nombre,
                        p.apellido,
                        p.telefono,
                        p.identidad,
                        p.direccion
                    })
                    .ToList();

                return Ok(cliente); // devuelve JSON automáticamente
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        // GET api/cliente/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var cliente = _context.Cliente.Find(id);

                if (cliente == null)
                {
                    return NotFound($"Cliente {id} no encontrado");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CrearCliente([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente != null)
                {

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    _context.Cliente.Add(cliente);
                    _context.SaveChanges();

                    return CreatedAtAction(nameof(GetById), new { id = cliente.id_cliente }, cliente);
                }

                return NotFound($"El Cliente no se pudo crear");
            }
            catch (Exception ex)
            {
                return NotFound("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Cliente model)
        {
            try
            {
                if (model != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var cliente = _context.Cliente.FirstOrDefault(x => x.id_cliente == id);

                    if (cliente != null)
                    {
                        cliente.id_cliente = id;
                        cliente.nombre = model.nombre;
                        cliente.apellido = model.apellido;
                        cliente.telefono = model.telefono;
                        cliente.identidad = model.identidad;
                        cliente.direccion = model.direccion;

                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound($"Cliente {id} no encontrado");
                    }

                    return Ok(cliente);
                }

                return NotFound($"El Cliente no se pudo editar");
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
                var cliente = _context.Cliente.FirstOrDefault(x => x.id_cliente == id);

                if (cliente != null)
                {
                    _context.Cliente.Remove(cliente);
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound($"Cliente {id} no encontrado");
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
