namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class ProductoVM
    {

        public int id_producto { get; set; }
        public string nombre { get; set; }
        public decimal? precio_compra_peso { get; set; } = 0;
        public decimal? precio_compra_dolar { get; set; } = 0;
        public decimal? precio_venta_peso { get; set; } = 0;
        public decimal? precio_venta_dolar { get; set; } = 0;
        public string codigo { get; set; }
        public int? minimo { get; set; } = 0;
        public int? maximo { get; set; } = 0;
        public string tipo { get; set; }
    }
}
