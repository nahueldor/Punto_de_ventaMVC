using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Models
{
    [Table("inventario")]
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Hace que el campo se auto-generé
        public int id_inventario { get; set; }

        public int producto { get; set; }

        public int cantidad { get; set; }

        public DateTime? fecha_inventario { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? tipo { get; set; }
    }
}
