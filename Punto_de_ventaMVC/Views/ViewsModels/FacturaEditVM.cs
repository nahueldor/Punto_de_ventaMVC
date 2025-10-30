using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class FacturaEditVM
    {
        public Venta venta { get; set; }
        public List<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
