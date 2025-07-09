using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class ActualizarPassword
    {
        public class EjecutaActualizarPassword : IRequest<bool>
        {
            public string NombreUsuario { get; set; }
            public string NuevaPassword { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutaActualizarPassword, bool>
        {
            private readonly ContextoLogin _contexto;

            public Manejador(ContextoLogin contexto)
            {
                _contexto = contexto;
            }

            public async Task<bool> Handle(EjecutaActualizarPassword request, CancellationToken cancellationToken)
            {
                var filter = Builders<modelo.Usuario>.Filter.Eq(u => u.NombreUsuario, request.NombreUsuario);
                var update = Builders<modelo.Usuario>.Update.Set(u => u.Password, request.NuevaPassword);

                var resultado = await _contexto.Usuarios.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

                return resultado.ModifiedCount > 0;
            }
        }
    }
}
