using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Models
{
    [Table("descripcion_factura")]
    public class FacturaDetalle
    {
        [Key]
        public int id_descripcion { get; set; }

        [ForeignKey("factura")]
        public Venta Venta { get; set; }

        [Required]
        public int producto { get; set; }

        [ForeignKey("producto")]
        public Producto Producto { get; set; }

        [Required]
        public int cantidad { get; set; }

        [Required]
        public int factura { get; set; }
               
        public decimal precio { get; set; }

    }
}