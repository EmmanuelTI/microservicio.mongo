using AutoMapper;
using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using uttt.edu.micro.loggin.modelo;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class Login
    {
        public class IniciarSesion : IRequest<UsuarioDto>
        {
            public string NombreUsuario { get; set; }
            public string Password { get; set; }
        }

        public class ManejadorIniciarSesion : IRequestHandler<IniciarSesion, UsuarioDto>
        {
            private readonly ContextoLogin _contexto;
            private readonly IMapper _mapper;

            public ManejadorIniciarSesion(ContextoLogin contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<UsuarioDto> Handle(IniciarSesion request, CancellationToken cancellationToken)
            {
                var usuario = await _contexto.Usuarios
                    .Find(u => u.NombreUsuario == request.NombreUsuario && u.Password == request.Password)
                    .FirstOrDefaultAsync(cancellationToken);

                if (usuario == null)
                {
                    throw new UnauthorizedAccessException("Nombre de usuario o contraseña incorrectos.");
                }

                return _mapper.Map<UsuarioDto>(usuario);
            }
        }
    }
}
