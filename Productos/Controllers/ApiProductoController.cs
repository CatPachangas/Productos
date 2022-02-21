using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productos.Models;
using Productos.Models.ViewModels;

namespace Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiProductoController : ControllerBase
    {
        private readonly DbProductoContext _context;

        public ApiProductoController(DbProductoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetProductos")]
        public async Task<List<Producto>> GetProductos()
        {
            var producto = _context.Producto.ToListAsync();
            return await producto;
        }


        [HttpPost]
        [Route("CrearProductos")]
        public async Task CrearProducto([FromBody] ProductoViewModel producto)
        {
            _context.Producto.Add(new Producto()
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Codigo = producto.Codigo,
                Estado = producto.Estado,
                Idcategoria = producto.Idcategoria,
                Imagen = producto.Imagen,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
            });
            _context.Add(producto);
            await _context.SaveChangesAsync();
        }

    }
}
