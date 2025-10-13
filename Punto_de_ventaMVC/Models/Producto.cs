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

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [MaxLength(50)]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El precio de compra del producto es obligatorio")]
        public decimal? precio_compra { get; set; } = 0;

        [Required(ErrorMessage = "El precio de venta del producto es obligatorio")]
        public decimal? precio_venta { get; set; } = 0;

        [Required(ErrorMessage = "El código del producto es obligatorio")]
        [MaxLength(20)]
        public string codigo { get; set; }

        [Required(ErrorMessage = "El stock minimo del producto es obligatorio")]
        public int? minimo { get; set; } = 0;

        [Required(ErrorMessage = "El stock maximo del producto es obligatorio")]
        public int? maximo { get; set; } = 0;

        [Required(ErrorMessage = "El tipo del producto es obligatorio")]
        [MaxLength(50)]
        public string tipo { get; set; }
    }
}
