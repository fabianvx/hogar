using Hogar.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Application.Services.Interfaces
{ 
       public interface IServiceNoticia
    {
        Task<ICollection<NoticiaDTO>> ListAsync();
        Task<NoticiaDTO> FindByIdAsync(int id);
        Task<int> AddAsync(NoticiaDTO dto);
        Task UpdateAsync(int id, NoticiaDTO dto);
        Task DeleteAsync(int id, NoticiaDTO dto);
    }
}
