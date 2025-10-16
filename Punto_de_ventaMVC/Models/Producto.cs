using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Models
{
    [Table("producto")]
    public class Producto : IValidatableObject
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

        [MaxLength(20)]
        public string codigo { get; set; } = "";

        [Required(ErrorMessage = "El stock minimo del producto es obligatorio")]
        public int? minimo { get; set; } = 0;

        [Required(ErrorMessage = "El stock maximo del producto es obligatorio")]
        public int? maximo { get; set; } = 0;

        [Required(ErrorMessage = "El tipo del producto es obligatorio")]
        [MaxLength(50)]
        public string tipo { get; set; }


        // Validación personalizada
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (minimo.HasValue && maximo.HasValue)
            {
                if (minimo > maximo)
                {
                    yield return new ValidationResult(
                        "El stock máximo debe ser mayor o igual al stock mínimo.",
                        new[] { nameof(maximo) } // campo afectado
                    );
                }
            }

            if (precio_compra.HasValue && precio_venta.HasValue)
            {
                if (precio_compra > precio_venta)
                {
                    yield return new ValidationResult(
                        "El precio de venta debe ser mayor o igual que el precio de compra.",
                        new[] { nameof(precio_venta) } // campo afectado
                    );
                }
            }
        }
    }
}
