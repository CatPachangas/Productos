using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Productos.Models.ViewModels;

namespace Productos.Models
{
    public class CategoriaController : Controller
    {

        private readonly DbProductoContext _context;

        public CategoriaController(DbProductoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
           => View(await _context.Categoria.ToListAsync());


        public static List<SelectListItem> ObtenerEstados()
        {
            var estado = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Activo", Value ="True"},
                new SelectListItem() {Text="Inactivo", Value ="False"},
            };

            return estado;
        }

        public IActionResult CrearCategorias()
        {
            ViewData["Estado"] = new SelectList(ObtenerEstados(), "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategorias(CategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categoria = new Categoria()
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Estado = model.Estado,
                };

                _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Estado"] = new SelectList(ObtenerEstados(), "Value", "Text", model.Estado);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Categoria> ObtenerCategoriaId(int Id)
        {
            var categoria = _context.Categoria.FirstOrDefault(x => x.IdCategoria == Id);
            return categoria;
        }

        public async Task<IActionResult> EditarCategoria(int Id)
        {
            var categoriaEditar = await ObtenerCategoriaId(Id).ConfigureAwait(false);

            if (categoriaEditar == null)
            {
                return NotFound();
            }

            ViewData["Estado"] = new SelectList(ObtenerEstados(), "Value", "Text");

            CategoriaViewModel categoria = new CategoriaViewModel()
            {
                IdCategoria = categoriaEditar.IdCategoria,
                Descripcion = categoriaEditar.Descripcion,
                Nombre = categoriaEditar.Nombre
            };


            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(CategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categoria = new Categoria()
                {
                    IdCategoria = model.IdCategoria,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre
                };

                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();
                TempData["mensaje"] = "La categoria se ha modificado";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categoria"] = new SelectList(_context.Categoria, "IdCategoria", "Nombre", model.IdCategoria);
            return View();
        }

        public async Task<IActionResult> EliminarCategoria(int Id)
        {
            var categoriaEliminar = await ObtenerCategoriaId(Id).ConfigureAwait(false);

            if (categoriaEliminar != null)
            {
                _context.Categoria.Remove(categoriaEliminar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }


}

