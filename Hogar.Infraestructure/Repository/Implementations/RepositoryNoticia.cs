using Hogar.Infraestructure.Data;
using Hogar.Infraestructure.Models;
using Hogar.Infraestructure.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Infraestructure.Repository.Implementations
{
    

    public class RepositoryNoticia : IRepositoryNoticia
    {
        private readonly HogarContext _context;
        public RepositoryNoticia(HogarContext context)
        {
            _context = context;
        }

        public async Task<Noticia> FindByIdAsync(int id)
        {

            var @object = await _context.Set<Noticia>().FindAsync(id);

            return @object!;
        }

        public async Task<ICollection<Noticia>> ListAsync()
        {
            var collection = await _context.Set<Noticia>().ToListAsync();
            return collection;
        }


        public async Task<int> AddAsync(Noticia entity)
        {
            // Insertar la nueva noticia en la tabla noticia usando SQL
            var sqlNoticia = @"
        INSERT INTO Noticia (Titulo, Descripcion,   Imagen, Estado, Usuario) 
        VALUES ( @Titulo, @Descripcion, @Imagen, @Estado, @Usuario);";

            // Parámetros para la consulta SQL de la noticia
            var parametersNoticia = new[]
            {                 
        new SqlParameter("@Titulo", entity.Titulo),
        new SqlParameter("@Descripcion", entity.Descripcion),
        new SqlParameter("@Imagen", entity.Imagen),
        new SqlParameter("@Estado", entity.Estado),
        new SqlParameter("@Usuario", entity.Usuario)
              };
            // Ejecutar la consulta y obtener el Id de la noticia recién insertada
            var NoticiaId = await _context.Database.ExecuteSqlRawAsync(sqlNoticia, parametersNoticia);       

            return NoticiaId; // Retornamos el Id de la Noticia creado
        }






        public async Task UpdateAsync(Noticia entity)
        {
            // Insertar la nueva noticia en la tabla noticia usando SQL
            var sqlNoticia = @"
        UPDATE Noticia SET Titulo = @Titulo, Descripcion = @Descripcion, Imagen = @Imagen, Estado  = @Estado  WHERE Id = @Id;";

            // Parámetros para la consulta SQL de la noticia
            var parametersNoticia = new[]
            {
                     new SqlParameter("@Id", entity.Id),
     new SqlParameter("@Titulo", entity.Titulo),
        new SqlParameter("@Descripcion", entity.Descripcion),
        new SqlParameter("@Imagen", entity.Imagen),
        new SqlParameter("@Estado", entity.Estado)
    };

            // Ejecutar la consulta y obtener el Id de la noticia recién insertado
            var noticiaId = await _context.Database.ExecuteSqlRawAsync(sqlNoticia, parametersNoticia); 

        }










        public async Task DeleteAsync(Noticia entity)
        {
           
            var id = entity.Id; // ID del producto

            var sql = "Delete from Noticia WHERE Id = @Id";

            // Usando SqlParameter para evitar el error de declaración de variable
            var parameters = new[]
            {    
             new SqlParameter("@Id", id)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }



    }
}
