using Microsoft.AspNetCore.Mvc.Rendering;
using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class AltaInventarioVM
    {
        public int ProductoId { get; set; }
        public List<SelectListItem> ProductosListItem { get; set; }
        public Inventario inventario { get; set; }
    }
}
