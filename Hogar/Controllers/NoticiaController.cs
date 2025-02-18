using Hogar.Application.DTOs;
using Hogar.Application.Services.Interfaces;
using Hogar.Infraestructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Hogar.Web.Controllers
{
    public class NoticiaController : Controller
    {

        private readonly IServiceNoticia _serviceNoticia;
        private readonly HogarContext context;


        public NoticiaController(IServiceNoticia serviceNoticia, HogarContext _context)
        {
            _serviceNoticia = serviceNoticia;
            context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceNoticia.ListAsync();
            ViewData["Title"] = "Index";
            return View(collection);
        }


        [Authorize]
 
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceNoticia.ListAsync();
            //Cantidad de elementos por página
            return View(collection.ToPagedList(page ?? 1, 5));
        }


        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceNoticia.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Noticia no existente");

                }

                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Create()
        {
            // Traer el último ID de producto y sumar 1
            var consecutivo = await context.Noticia.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            ViewBag.id = consecutivo?.Id + 1 ?? 1; // Asigna el ID inicial como 1 si no hay productos

            return View();
        }



        // POST: ProductoController/Create
        [HttpPost]

        [Authorize]
 
        public async Task<ActionResult> Create(NoticiaDTO dto, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null)
                {
                    // Validar formato del archivo
                    var allowedFormats = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLower();

                    if (!allowedFormats.Contains(extension))
                    {
                        ModelState.AddModelError("Imagen", "Formato de imagen no permitido. Solo se permiten imágenes .jpg, .jpeg, .png y .gif.");
                        return View(dto);
                    }
                    //Gestión de imagen
                    MemoryStream target = new MemoryStream();

                    if (dto.Imagen == null && imageFile != null)
                    {
                        imageFile.OpenReadStream().CopyTo(target);
                        dto.Imagen = target.ToArray();
                        ModelState.Remove("Imagen");
                    }
                }

                try
                {
                    //Validación del formulario
                    if (!ModelState.IsValid)
                    {
                        // Lee del ModelState todos los errores que
                        // vienen para el lado del server
                        string errors = string.Join("; ", ModelState.Values
                                           .SelectMany(x => x.Errors)
                                           .Select(x => x.ErrorMessage));
                        ViewBag.ErrorMessage = errors;
                        return View();
                    }
                    dto.Estado = 1;
                    dto.Usuario = "0";
                    //Crear
                    await _serviceNoticia.AddAsync(dto);
                    return RedirectToAction("IndexAdmin");
                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException?.Message ?? ex.Message;
                    ViewBag.ErrorMessage = $"Error al guardar la noticia: {innerException}";        
                    return View(dto);
                }
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                ViewBag.ErrorMessage = $"Error al guardar el noticia: {innerException}";
                return View(dto);
            }
        }
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {       

            var @object = await _serviceNoticia.FindByIdAsync(id);
            @object.ImagenEdit = @object.Imagen; // Copiar la imagen actual          
            return View(@object);

        }



        // POST: ProductoController/Edit/5
        [HttpPost]

        [Authorize]
 
        public async Task<ActionResult> Edit(int id, IFormFile imageFile, NoticiaDTO dto)
        {
            try
            {
                // Si se proporciona una nueva imagen
                if (imageFile != null)
                {
                    var allowedFormats = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLower();

                    if (!allowedFormats.Contains(extension))
                    {
                        ModelState.AddModelError("Imagen", "Formato de imagen no permitido.");
                        return View(dto);
                    }

                    using (MemoryStream target = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(target);
                        dto.Imagen = target.ToArray();
                    }
                }
                else
                {
                    // Si no se proporciona una nueva imagen,  se usa la que ya estaba antes                

                    dto.Imagen = dto.ImagenEdit;

                }

                ModelState.Remove("imageFile");               
                if (!ModelState.IsValid)
                {
                    // Lee del ModelState todos los errores que vienen para el lado del servidor
                    string errors = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                    ViewBag.ErrorMessage = errors;
                    return View(dto);
                }
                else
                { 
                    await _serviceNoticia.UpdateAsync(id, dto);
                    return RedirectToAction("IndexAdmin");
                }

            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                ViewBag.ErrorMessage = $"Error al actualizar la noticia: {innerException}";
                // Redirigir a IndexAdmin en caso de error
                return RedirectToAction("IndexAdmin");
            }

        }

    
        [HttpPost]
        [Authorize]
 
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var producto = await _serviceNoticia.FindByIdAsync(id);
                if (producto == null)
                {
                    throw new Exception("Noticia no existente");
                }           
                await _serviceNoticia.DeleteAsync(id, producto);

                // Redirigir a IndexAdmin después de eliminar
                return RedirectToAction("IndexAdmin");
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                ViewBag.ErrorMessage = $"Error al eliminar la noticia: {innerException}";

                // Redirigir a IndexAdmin en caso de error
                return RedirectToAction("IndexAdmin");
            }
        }


    }

}
