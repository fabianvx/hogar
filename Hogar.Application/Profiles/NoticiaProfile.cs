using AutoMapper;
using Hogar.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hogar.Infraestructure.Models;
 

namespace Hogar.Application.Profiles
{
    public class NoticiaProfile:Profile
    {
        public NoticiaProfile()
        {
            CreateMap<NoticiaDTO, Noticia>().ReverseMap();
        }
    }
}
