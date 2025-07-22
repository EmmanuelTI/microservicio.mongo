using MediatR;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using uttt.edu.micro.loggin.modelo;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class Logout
    {
        public class CerrarSesion : IRequest
        {
            public Guid UsuarioId { get; set; }
        }

        public class ManejadorCerrarSesion : IRequestHandler<CerrarSesion>
        {
            private readonly ContextoLogin _contexto;

            public ManejadorCerrarSesion(ContextoLogin contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(CerrarSesion request, CancellationToken cancellationToken)
            {
                var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, request.UsuarioId);
                var update = Builders<Usuario>.Update
                    .Set(u => u.RefreshToken, null)
                    .Set(u => u.RefreshTokenExpiry, DateTime.MinValue);

                var resultado = await _contexto.Usuarios.UpdateOneAsync(filtro, update, cancellationToken: cancellationToken);

                if (resultado.MatchedCount == 0)
                    throw new Exception("Usuario no encontrado.");

                return Unit.Value;
            }
        }
    }
}
