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

        [Display(Name ="Numero de Factura")]
        [Required(ErrorMessage ="El campo numero, es obligatorio")]
        public int? numero { get; set;}

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El campo cliente, es obligatorio")]
        public int? cliente { get; set; } = 0;


        //[ForeignKey("cliente")]
        //public Cliente Cliente { get; set; }  // Propiedad de navegació

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo usuario, es obligatorio")]
        public int? usuario { get; set; } = 0;

        [Display(Name = "Fecha de Factura")]
        public DateTime? fecha_facturacion { get; set; } = DateTime.Now;

        [Display(Name = "Total")]
        public decimal? total { get; set; } = 0;

        [Display(Name = "Subtotal")]
        public decimal? subtotal { get; set; } = 0;

        [Display(Name = "ISV")]
        public decimal? isv { get; set; } = 0;

        [Display(Name = "Descuento")]
        public decimal? descuento { get; set; } = 0;

        //public virtual ICollection<FacturaDetalle> Detalles { get; set; }

    }
}
