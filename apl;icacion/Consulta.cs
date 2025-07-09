using AutoMapper;
using MediatR;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using uttt.edu.micro.loggin.modelo;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class Consulta
    {
        // ✅ Consulta para obtener todos los usuarios
        public class ListaUsuarios : IRequest<List<UsuarioDto>> { }

        public class ManejadorListaUsuarios : IRequestHandler<ListaUsuarios, List<UsuarioDto>>
        {
            private readonly ContextoLogin _contexto;
            private readonly IMapper _mapper;

            public ManejadorListaUsuarios(ContextoLogin contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<UsuarioDto>> Handle(ListaUsuarios request, CancellationToken cancellationToken)
            {
                var usuarios = await _contexto.Usuarios.Find(_ => true).ToListAsync(cancellationToken);
                return _mapper.Map<List<UsuarioDto>>(usuarios);
            }
        }

        // ✅ Consulta para obtener un usuario por nombre
        public class UsuarioPorNombre : IRequest<UsuarioDto>
        {
            public string NombreUsuario { get; set; }
        }

        public class ManejadorUsuarioPorNombre : IRequestHandler<UsuarioPorNombre, UsuarioDto>
        {
            private readonly ContextoLogin _contexto;
            private readonly IMapper _mapper;

            public ManejadorUsuarioPorNombre(ContextoLogin contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<UsuarioDto> Handle(UsuarioPorNombre request, CancellationToken cancellationToken)
            {
                var usuario = await _contexto.Usuarios
                    .Find(u => u.NombreUsuario == request.NombreUsuario)
                    .FirstOrDefaultAsync(cancellationToken);

                return _mapper.Map<UsuarioDto>(usuario);
            }
        }
    }
}
