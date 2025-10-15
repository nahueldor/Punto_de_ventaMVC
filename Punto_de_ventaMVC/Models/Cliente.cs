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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres")]
        public string apellido { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [MaxLength(50, ErrorMessage = "El teléfono no puede superar los 50 caracteres")]
        public string telefono { get; set; }

        [Required(ErrorMessage = "La identidad es obligatoria")]
        [MaxLength(50, ErrorMessage = "La identidad no puede superar los 50 caracteres")]
        public string identidad { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [MaxLength(100, ErrorMessage = "La dirección no puede superar los 100 caracteres")]
        public string direccion { get; set; }
    }
}
