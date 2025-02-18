 
using Hogar.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryTipoUsuario
    {
        Task<ICollection<TipoUsuario>> ListAsync();
        Task<TipoUsuario> FindByIdAsync(int id);
    }
}
