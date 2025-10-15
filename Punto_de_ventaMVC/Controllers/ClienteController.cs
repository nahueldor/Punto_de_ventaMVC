using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            try
            {
                var datos = _context.Cliente.ToList();
                return View(datos);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        public IActionResult AltaCliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AltaCliente(Cliente cliente)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        public IActionResult ModificarCliente(int id)
        {
            try
            {

                var cliente = _context.Cliente.FirstOrDefault(x => x.id_cliente == id) ?? throw new ArgumentException($"Cliente {id} no encontrado.");

                return View(cliente);
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
        public async Task<IActionResult> ModificarCliente(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                // Guardar mensaje de error en ViewBag o ViewData
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error"); // Redirige a la vista Error.cshtml
            }
        }

        [HttpGet]
        public async Task<IActionResult> BajarCliente(int id)
        {
            try
            {
                var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.id_cliente == id);

                if (cliente == null)
                {
                    throw new ArgumentException($"Cliente {id} no encontrado.");
                }

                _context.Cliente.Remove(cliente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error");
            }
        }
    }
}
