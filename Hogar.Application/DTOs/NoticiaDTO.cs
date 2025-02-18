using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Application.DTOs
{
    public record NoticiaDTO
    {
        //ID Ingrediente
        [Display(Name = "Código")]
        public int Id { get; set; }

        //ID Ingrediente
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Titulo { get; set; } = null!;

        //Descripción Ingrediente
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; } = null!;

        [ValidateNever]
        //Imagen de la Noticia
        [Display(Name = "Imagen")]     
        public byte[] Imagen { get; set; } = null!;

        [ValidateNever]
        public byte[] ImagenEdit { get; set; } = null!;

        //Estado de la Noticia: 1 Activo | 2 Inactivo 
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Estado es un dato requerido")]
        public int Estado { get; set; }
        public string EstadoDescripcion
        {
            get
            {
                return Estado == 1 ? "Activo" : "Inactivo";
            }
        }

        [ValidateNever]
        public string Usuario { get; set; } = null!;

 

    }
}
