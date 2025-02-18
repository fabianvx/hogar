using Hogar.Infraestructure.Data;
using Hogar.Infraestructure.Models;
using Hogar.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Infraestructure.Repository.Implementations
{
    public class RepositoryTipoUsuario:IRepositoryTipoUsuario
    {
        private readonly HogarContext _context;
        public RepositoryTipoUsuario(HogarContext context) {
            _context=context;
        }

        public async Task<TipoUsuario> FindByIdAsync(int id)
        {

            var @object = await _context.Set<TipoUsuario>().FindAsync(id);

            return @object!;
        }

        public async Task<ICollection<TipoUsuario>> ListAsync()
        {
            var collection = await _context.Set<TipoUsuario>().ToListAsync();
            return collection;
        }
    }
}
