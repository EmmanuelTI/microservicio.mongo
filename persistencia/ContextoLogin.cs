using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using uttt.edu.micro.loggin.modelo;

namespace uttt.edu.micro.loggin.persistencia
{
    public class ContextoLogin
    {
        private readonly IMongoDatabase _database;

        public ContextoLogin(IConfiguration configuration)
        {
            var cliente = new MongoClient(configuration.GetConnectionString("MongoConnection"));
            _database = cliente.GetDatabase("LoginDb"); 
        }

        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
    }
}
