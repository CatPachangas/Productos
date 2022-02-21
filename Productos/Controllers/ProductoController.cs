using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Productos.Models;
using Productos.Models.ViewModels;

namespace Productos.Controllers
{
    public class ProductoController : Controller
    {

        private readonly DbProductoContext _context;

        public ProductoController(DbProductoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var producto = _context.Producto.Include(info => info.IdcategoriaNavigation);
            return View(await producto.ToListAsync());
        }

        public static List<SelectListItem> ObtenerEstados()
        {
            var estado = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Activo", Value ="True"},
                new SelectListItem() {Text="Inactivo", Value ="False"},

            };

            return estado;
        }

        public IActionResult CrearProducto()
        {
            ViewData["Categoria"] = new SelectList(_context.Categoria, "IdCategoria", "Nombre");
            ViewData["Estado"] = new SelectList(ObtenerEstados(), "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProducto(ProductoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var producto = new Producto()
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    PrecioVenta = model.PrecioVenta,
                    Codigo = model.Codigo,
                    Imagen = model.Imagen,
                    Idcategoria = model.Idcategoria,
                    Estado = model.Estado,
                    Stock = model.Stock,
                };

                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();
                TempData["mensaje"] = "El producto se ha creado";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categoria"] = new SelectList(_context.Categoria, "IdCategoria", "Nombre", model.Idcategoria);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Producto> ObtenerProductoId(int Id)
        {
            var producto = _context.Producto.FirstOrDefault(x => x.Idproducto == Id);
            return producto;
        }

        public async Task<IActionResult> EliminarProducto(int Id)
        {
            var productoEliminar = await ObtenerProductoId(Id).ConfigureAwait(false);

            if (productoEliminar != null)
            {
                _context.Producto.Remove(productoEliminar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public async Task<IActionResult> EditarProducto(int Id)
        {
            var productoEditar = await ObtenerProductoId(Id).ConfigureAwait(false);

            if (productoEditar == null)
            {
                return NotFound();
            }

            ViewData["Categoria"] = new SelectList(_context.Categoria, "IdCategoria", "Nombre");
            ViewData["Estado"] = new SelectList(ObtenerEstados(), "Value", "Text");

            ProductoViewModel producto = new ProductoViewModel()
            {
                Codigo = productoEditar.Codigo,
                Descripcion = productoEditar.Descripcion,
                IdProducto = productoEditar.Idproducto,
                Imagen = productoEditar.Imagen,
                Nombre = productoEditar.Nombre,
                PrecioVenta = productoEditar.PrecioVenta,
                Stock = productoEditar.Stock,
            };


            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProducto(ProductoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var producto = new Producto()
                {
                    Idproducto = model.IdProducto,
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    PrecioVenta = model.PrecioVenta,
                    Codigo = model.Codigo,
                    Imagen = model.Imagen,
                    Idcategoria = model.Idcategoria,
                    Estado = model.Estado,
                    Stock = model.Stock,
                };

                _context.Producto.Update(producto);
                await _context.SaveChangesAsync();
                TempData["mensaje"] = "El producto se ha modificado";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categoria"] = new SelectList(_context.Categoria, "IdCategoria", "Nombre", model.Idcategoria);
            return View();
        }

    }
}
