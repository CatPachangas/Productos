using System.ComponentModel.DataAnnotations;

namespace Productos.Models.ViewModels
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }

        [Display(Name = "Codigo")]
        public string? Codigo { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal PrecioVenta { get; set; }

        [Required]
        public int Stock { get; set; }

        public byte[]? Imagen { get; set; }

        public bool? Estado { get; set; }

        [Display(Name = "Categoria")]
        public int Idcategoria { get; set; }
    }
}
