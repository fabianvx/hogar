using Burger.Application.DTOs;
using Hogar.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Application.Services.Interfaces
{
    public interface IServiceTipoUsuario
    {
        Task<ICollection<TipoUsuarioDTO>> ListAsync();
        Task<TipoUsuarioDTO> FindByIdAsync(int id);
    }
}
