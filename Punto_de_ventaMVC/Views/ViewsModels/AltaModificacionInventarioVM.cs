using Microsoft.AspNetCore.Mvc.Rendering;
using Punto_de_ventaMVC.Models;

namespace Punto_de_ventaMVC.Views.ViewsModels
{
    public class AltaModificacionInventarioVM
    {
        public List<SelectListItem> ProductosListItem { get; set; }
        public Inventario inventario { get; set; }
    }
}
