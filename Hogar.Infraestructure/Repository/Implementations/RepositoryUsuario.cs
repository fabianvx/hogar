using Hogar.Infraestructure.Data;
using Hogar.Infraestructure.Models;
using Hogar.Infraestructure.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 


namespace Hogar.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuario:IRepositoryUsuario
    {
        private readonly HogarContext _context;
        public RepositoryUsuario(HogarContext context) {
            _context=context;
        }

        public async Task<Usuario> FindByIdAsync(string id)
        {

            var @object = await _context.Set<Usuario>()
             .Include(p => p.TipoNavigation) // Incluye el tipo de usuario   
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

            return @object!;
        }

        public async Task<ICollection<Usuario>> ListAsync()
        {
            var collection = await _context.Set<Usuario>()
                .Include(p => p.TipoNavigation) // Incluye el tipo de usuario
                .ToListAsync();
            return collection;
        }

        //public async Task<Usuario> LoginAsync(string id, string password)
        //{
        //    var @object = await _context.Set<Usuario>()
        //                                .Include(b => b.TipoNavigation)
        //                                .Where(p => p.Id == id && p.Contraseña == password)
        //                                .FirstOrDefaultAsync();
        //    return @object!;
        //}

        public async Task<Usuario> LoginAsync(string id, string password)
        {
            var user = await _context.Set<Usuario>()
                                     .Include(b => b.TipoNavigation)
                                     .Where(p => p.Id == id)
                                     .FirstOrDefaultAsync();

            // Verificar si el usuario existe y comparar la contraseña ingresada con la almacenada
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Contraseña))
            {
                return user;
            }

            return null!;
        }


        public async Task<int> AddAsync(Usuario entity)
        {
            // Hashear la contraseña antes de insertarla en la base de datos
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(entity.Contraseña);

            // Insertar el nuevo usuario en la tabla Usuario usando SQL
            var sqlUsuario = @"
        INSERT INTO Usuario (Id, Nombre, Apellidos, Correo, Contraseña, Tipo, Estado) 
        VALUES (@Id, @Nombre, @Apellidos, @Correo, @Contraseña, @Tipo, @Estado);";

            // Parámetros para la consulta SQL del usuario
            var parametersUsuario = new[]
            {
        new SqlParameter("@Id", entity.Id),
        new SqlParameter("@Nombre", entity.Nombre),
        new SqlParameter("@Apellidos", entity.Apellidos),
        new SqlParameter("@Correo", entity.Correo),
        new SqlParameter("@Contraseña", hashedPassword), // Guardamos la contraseña hasheada
        new SqlParameter("@Tipo", entity.Tipo),
        new SqlParameter("@Estado", entity.Estado)
    };

            // Ejecutar la consulta y obtener el Id del usuario recién insertado
            var UsuarioID = await _context.Database.ExecuteSqlRawAsync(sqlUsuario, parametersUsuario);

            return UsuarioID; // Retornamos el Id del usuario creado
        }


        public async Task UpdateAsync(string id, Usuario entity)
        {
            // Obtener la contraseña actual del usuario desde la base de datos
            var currentPasswordHash = await _context.Set<Usuario>()
                                                    .Where(u => u.Id == id)
                                                    .Select(u => u.Contraseña)
                                                    .FirstOrDefaultAsync();

            //// Solo hashear si la contraseña es diferente de la almacenada
            //if (!BCrypt.Net.BCrypt.Verify(entity.Contraseña, currentPasswordHash))
            //{
            //    entity.Contraseña = BCrypt.Net.BCrypt.HashPassword(entity.Contraseña);
            //}
            //else
            //{
            //    entity.Contraseña = currentPasswordHash;
            //}

            // Consulta SQL para actualizar los datos del usuario en la tabla Usuario
            var sqlUsuario = @"
        UPDATE Usuario 
        SET 
            Nombre = @Nombre, 
            Apellidos = @Apellidos, 
            Correo = @Correo, 
            Contraseña = @Contraseña, 
            Tipo = @Tipo, 
            Estado = @Estado
        WHERE Id = @Id;";

            var parametersUsuario = new[]
            {
        new SqlParameter("@Id", entity.Id),
        new SqlParameter("@Nombre", entity.Nombre),
        new SqlParameter("@Apellidos", entity.Apellidos),
        new SqlParameter("@Correo", entity.Correo),
        new SqlParameter("@Contraseña", entity.Contraseña),
        new SqlParameter("@Tipo", entity.Tipo),
        new SqlParameter("@Estado", entity.Estado)
    };

            await _context.Database.ExecuteSqlRawAsync(sqlUsuario, parametersUsuario);
        }



        public async Task DeleteAsync(string Id, Usuario entity)
        {
  
            var id = entity.Id; // ID del Usuario

            var sql = "DELETE FROM Usuario WHERE Id = @Id";

            // Usando SqlParameter para evitar el error de declaración de variable
            var parameters = new[]
            {
 
        new SqlParameter("@Id", id)
                         };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }


    }
}
