using Punto_de_ventaMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class FacturaVM
    {
        public VentaVM Venta { get; set; }
        public List<FacturaDetalleVM> Detalles { get; set; } = new List<FacturaDetalleVM>();



        public class VentaVM
        {
            public int id_factura { get; set; }
            public int numero { get; set; }
            public int? cliente { get; set; } = 0;
            public int? usuario { get; set; } = 0;
            public DateTime? fecha_facturacion { get; set; } = DateTime.Now;
            public decimal? total { get; set; } = 0;
            public decimal? subtotal { get; set; } = 0;
            public decimal? isv { get; set; } = 0;
            public decimal? descuento { get; set; } = 0;
        }

        public class FacturaDetalleVM
        {
            public int id_descripcion { get; set; }
            public int producto { get; set; }
            public int cantidad { get; set; }
            public int factura { get; set; }
            public decimal precio { get; set; }
        }
    }
}
