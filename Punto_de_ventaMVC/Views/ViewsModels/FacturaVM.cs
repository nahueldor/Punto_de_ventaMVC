using Punto_de_ventaMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class FacturaVM
    {
        public Venta Venta { get; set; }
        public List<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }
}
