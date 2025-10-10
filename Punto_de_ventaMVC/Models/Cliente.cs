using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Punto_de_ventaMVC.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto incremento
        public int id_cliente { get; set; }

        [Required]
        [MaxLength(50)]
        public string nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string apellido { get; set; }

        [MaxLength(50)]
        public string telefono { get; set; }

        [MaxLength(50)]
        public string identidad { get; set; }

        [MaxLength(100)]
        public string direccion { get; set; }
    }
}
