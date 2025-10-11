using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Models
{

    [Table("factura")]
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto incremento
        public int id_factura { get; set; }

        [Required]
        public int? numero { get; set; } = 0;

        public int? cliente { get; set; } = 0;

        public int? usuario { get; set; } = 0;

        public DateTime? fecha_facturacion { get; set; } = DateTime.Now;

        public decimal? total { get; set; } = 0;

        public decimal? subtotal { get; set; } = 0;

        public decimal? isv { get; set; } = 0;

        public decimal? descuento { get; set; } = 0;

    }
}
