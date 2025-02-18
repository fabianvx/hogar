using System;
using System.Collections.Generic;

namespace Hogar.Infraestructure.Models;

public partial class Usuario
{
    public string Id { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int Tipo { get; set; }

    public int Estado { get; set; }

    public virtual TipoUsuario TipoNavigation { get; set; } = null!;
}
