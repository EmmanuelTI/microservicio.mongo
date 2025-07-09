using AutoMapper;
using uttt.edu.micro.loggin.aplicacion;
using uttt.edu.micro.loggin.modelo;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Usuario, UsuarioDto>();

        }
    }
}
