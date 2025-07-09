using MongoDB.Bson.Serialization.Attributes;
using System;

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
    }
}
