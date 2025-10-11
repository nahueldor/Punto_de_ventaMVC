using Punto_de_ventaMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class InventarioVM
    {
        public int id_inventario { get; set; }

        public int codigo { get; set; }

        public string? producto { get; set; }

        public int cantidad { get; set; }

        public DateTime? fecha_inventario { get; set; }

        public string? tipo { get; set; }

        public int? stock_minimo { get; set; }

        public int? stock_maximo { get; set; }
    }
}
