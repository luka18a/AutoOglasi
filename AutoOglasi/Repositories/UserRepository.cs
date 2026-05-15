using AutoOglasi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace AutoOglasi.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
            _users = database.GetCollection<User>("Users");
        }

        public User? GetByUsername(string username)
        {
            return _users.Find(u => u.Username == username).FirstOrDefault();
        }

        public User? GetByEmail(string email)
        {
            return _users.Find(u => u.Email == email).FirstOrDefault();
        }

        public void Create(User user)
        {
            _users.InsertOne(user);
        }
    }
}