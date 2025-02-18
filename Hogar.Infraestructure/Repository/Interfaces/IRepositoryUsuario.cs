 
using Hogar.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<ICollection<Usuario>> ListAsync();
        Task<Usuario> FindByIdAsync(string id);
        Task<Usuario> LoginAsync(string id, string password);
        Task<int> AddAsync(Usuario entity);
        Task UpdateAsync(string id, Usuario dto);

        Task DeleteAsync(string id, Usuario dto);
    }
}
