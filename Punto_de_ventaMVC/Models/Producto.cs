using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Models
{
    [Table("producto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto incremento
        public int id_producto { get; set; }

        [Required]
        [MaxLength(50)]
        public string nombre { get; set; }

        public decimal? precio_compra { get; set; } = 0;

        public decimal? precio_venta { get; set; } = 0;

        [MaxLength(20)]
        public string codigo { get; set; }

        public int? minimo { get; set; } = 0;

        public int? maximo { get; set; } = 0;

        [MaxLength(50)]
        public string tipo { get; set; }
    }
}
