using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class DatailsVM
    {
        public int id_factura { get; set; }
        public int? numero { get; set; }
        public string cliente { get; set; }    // nombre completo del cliente
        public string usuario { get; set; }    // nombre de usuario (string)
        public DateTime? fecha { get; set; }
        public decimal? total { get; set; }
        public decimal? subtotal { get; set; }
        public decimal? isv { get; set; }
        public decimal? descuento { get; set; }
        public List<ProductoDetalle>? productosDetalle { get; set; }

        public class ProductoDetalle
        {
            public FacturaDetalle detalle { get; set; }
            public Producto producto { get; set; }
        }
    }
}
