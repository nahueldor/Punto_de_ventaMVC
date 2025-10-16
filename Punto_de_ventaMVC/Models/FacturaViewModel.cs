using Microsoft.AspNetCore.Mvc.Rendering;

namespace Punto_de_ventaMVC.Models
{
    public class FacturaViewModel
    {

        public Venta Venta { get; set; }
        public List<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();

        public List<SelectListItem> Clientes { get; set; }
        public List<SelectListItem> Usuarios { get; set; }
        public List<SelectListItem> Productos { get; set; }

    }
}
