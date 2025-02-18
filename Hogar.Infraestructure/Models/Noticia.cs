using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hogar.Infraestructure.Models;

public partial class Noticia
{
    [Key]
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public byte[] Imagen { get; set; } = null!;

    public int Estado { get; set; }

    public string Usuario { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
