using AutoMapper;
using Hogar.Application.DTOs;
using Hogar.Application.Services.Interfaces;
using Hogar.Infraestructure.Models;
using Hogar.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hogar.Application.Services.Implementations
{
   

    public class ServiceNoticia : IServiceNoticia
    {
        private readonly IRepositoryNoticia _repository;
        private readonly IMapper _mapper;
        public ServiceNoticia(IRepositoryNoticia repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<NoticiaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<NoticiaDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<NoticiaDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<NoticiaDTO>>(list);
            // Return lista
            return collection;
        }

        public async Task<int> AddAsync(NoticiaDTO dto)
        {
            var objectMapped = _mapper.Map<Noticia>(dto);
            return await _repository.AddAsync(objectMapped);
        }


        public async Task UpdateAsync(int id, NoticiaDTO dto)
        {
            var @object = await _repository.FindByIdAsync(id);
            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id, NoticiaDTO dto)
        {
            var @object = await _repository.FindByIdAsync(id);
            var entity = _mapper.Map(dto, @object!);

            await _repository.DeleteAsync(entity);
        }


    }
}
