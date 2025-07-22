using MongoDB.Bson.Serialization.Attributes;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace uttt.edu.micro.loggin.modelo
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string PreguntaSecreta { get; set; }
        public string RespuestaSecreta { get; set; }

        public string RefreshToken { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime RefreshTokenExpiry { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public string GenerarJwt(string claveSecreta, string issuer, string audience, int minutosExpiracion = 60)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(claveSecreta);


            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
        new Claim(ClaimTypes.Name, NombreUsuario)
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(minutosExpiracion),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public string GenerarRefreshToken(int tamañoBytes = 32)
        {
            var randomBytes = new byte[tamañoBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public void AsignarNuevoRefreshToken()
        {
            RefreshToken = GenerarRefreshToken();
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);
        }
    }
}
