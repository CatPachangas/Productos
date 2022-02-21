using System.ComponentModel.DataAnnotations;

namespace Productos.Models.ViewModels
{
    public class CategoriaViewModel
    {

        public int IdCategoria { get; set; }
        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public bool? Estado { get; set; }
    }
}
