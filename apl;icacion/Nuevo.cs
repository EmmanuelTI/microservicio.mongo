using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using uttt.edu.micro.loggin.modelo;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string NombreUsuario { get; set; }
            public string Password { get; set; }
            public string PreguntaSecreta { get; set; }
            public string RespuestaSecreta { get; set; }
        }

        public class EjecutarValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutarValidacion()
            {
                RuleFor(x => x.NombreUsuario).NotEmpty().WithMessage("El nombre de usuario es obligatorio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es obligatoria");
                RuleFor(x => x.PreguntaSecreta).NotEmpty().WithMessage("La pregunta secreta es obligatoria");
                RuleFor(x => x.RespuestaSecreta).NotEmpty().WithMessage("La respuesta secreta es obligatoria");
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLogin _contexto;

            public Manejador(ContextoLogin contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var nuevoUsuario = new Usuario
                {
                    NombreUsuario = request.NombreUsuario,
                    Password = request.Password,
                    PreguntaSecreta = request.PreguntaSecreta,
                    RespuestaSecreta = request.RespuestaSecreta
                };

                await _contexto.Usuarios.InsertOneAsync(nuevoUsuario, cancellationToken: cancellationToken);

                return Unit.Value;
            }
        }
    }
}
