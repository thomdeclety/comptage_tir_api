using ComptageTirAPI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComptageTirAPI.Services
{
    public class ArcheryService
    {
        private readonly IMongoCollection<User> _users;

        public ArcheryService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoConnection"));
            var database = client.GetDatabase("comptage_tir");
            _users = database.GetCollection<User>("users");
        }

        public void AddAccount(string username, string password)
        {
            try
            {
                _users.InsertOne(new User() { UserName = username, Password = password });
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string Connect(string username, string password)
        {
            return "ok";
        }
    }
}
