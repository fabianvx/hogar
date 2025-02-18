using System;
using Hogar.Application.DTOs;
using Hogar.Infraestructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hogar.Infraestructure.Models;
using Burger.Application.DTOs;

namespace Hogar.Application.Profiles
{
    public class TipoUsuarioProfile : Profile
    {
        public TipoUsuarioProfile()
        {
            CreateMap<TipoUsuarioDTO, TipoUsuario>().ReverseMap();
        }
    }
}
