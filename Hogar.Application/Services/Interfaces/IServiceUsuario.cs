using Hogar.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<ICollection<UsuarioDTO>> ListAsync();
        Task<UsuarioDTO> FindByIdAsync(string id);
        Task<int> AddAsync(UsuarioDTO dto);
        Task UpdateAsync(string id, UsuarioDTO dto);
        Task<UsuarioDTO> AuthenticateAsync(string username, string password);
        Task DeleteAsync(string id, UsuarioDTO dto);

    }
}
