using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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

        [Required(ErrorMessage ="El campo numero, es obligatorio")]
        public int? numero { get; set;}

        [Required]
        public int? cliente { get; set; } = 0;


        //[ForeignKey("cliente")]
        //public Cliente Cliente { get; set; }  // Propiedad de navegació

        [Required]
        public int? usuario { get; set; } = 0;

        public DateTime? fecha_facturacion { get; set; } = DateTime.Now;

        public decimal? total { get; set; } = 0;

        public decimal? subtotal { get; set; } = 0;

        public decimal? isv { get; set; } = 0;

        public decimal? descuento { get; set; } = 0;

        //public virtual ICollection<FacturaDetalle> Detalles { get; set; }

    }
}
