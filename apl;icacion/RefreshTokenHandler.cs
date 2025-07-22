using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using uttt.edu.micro.loggin.config;
using uttt.edu.micro.loggin.modelo;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.aplicacion
{
    public class RefreshTokenHandler
    {
        public class RenovarTokenRequest : IRequest<LoginResponseDto>
        {
            public string RefreshToken { get; set; }
        }

        public class ManejadorRenovarToken : IRequestHandler<RenovarTokenRequest, LoginResponseDto>
        {
            private readonly ContextoLogin _contexto;
            private readonly IMapper _mapper;
            private readonly JwtSettings _jwtSettings;

            public ManejadorRenovarToken(ContextoLogin contexto, IMapper mapper, IOptions<JwtSettings> jwtSettings)
            {
                _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            }

            public async Task<LoginResponseDto> Handle(RenovarTokenRequest request, CancellationToken cancellationToken)
            {
                var usuario = await _contexto.Usuarios
                    .Find(u => u.RefreshToken == request.RefreshToken && u.RefreshTokenExpiry > DateTime.UtcNow)
                    .FirstOrDefaultAsync(cancellationToken);

                if (usuario == null)
                    throw new UnauthorizedAccessException("Refresh token inválido o expirado.");

                usuario.AsignarNuevoRefreshToken();

                var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id);
                var update = Builders<Usuario>.Update
                    .Set(u => u.RefreshToken, usuario.RefreshToken)
                    .Set(u => u.RefreshTokenExpiry, usuario.RefreshTokenExpiry);

                await _contexto.Usuarios.UpdateOneAsync(filtro, update, cancellationToken: cancellationToken);

           
                string nuevoToken = usuario.GenerarJwt(
                    claveSecreta: _jwtSettings.SecretKey,
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    minutosExpiracion: 60);

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                return new LoginResponseDto
                {
                    Usuario = usuarioDto,
                    Token = nuevoToken
                };
            }
        }
    }
}
