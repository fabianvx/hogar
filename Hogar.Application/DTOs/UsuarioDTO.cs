using Hogar.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Application.DTOs
{
    public record UsuarioDTO
    {
        //ID Usuario
        [Display(Name = "Identificación")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Id { get; set; } = null!;


        //Nombre Usuario
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;


        //Apellidos Usuario
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Apellidos { get; set; } = null!;

        //Correo Usuario
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Correo { get; set; } = null!;

        //Contraseña Usuario
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Contraseña { get; set; } = null!;

        [ValidateNever]
        public string Contraseña2 { get; set; } = null!;

        //Rol Usuario
        [Display(Name = "Rol")]
        [ValidateNever]
        public virtual TipoUsuario TipoNavigation { get; set; } = null!;

        [ValidateNever]
        public int Tipo { get; set; }

        //Estado del Combo: 1 Activo | 2 Inactivo 
        [Display(Name = "Estado")]
        public int Estado { get; set; }
        public string EstadoDescripcion
        {
            get
            {
                return Estado == 1 ? "Activo" : "Inactivo";
            }
        }

      

 

    }
}
