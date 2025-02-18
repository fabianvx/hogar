using Hogar.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Infraestructure.Repository.Interfaces
{
  
      public interface IRepositoryNoticia
    {
        Task<ICollection<Noticia>> ListAsync();
        Task<Noticia> FindByIdAsync(int id);
        Task<int> AddAsync(Noticia dto);
        Task UpdateAsync( Noticia dto);
        Task DeleteAsync(Noticia dto);
    }
}
