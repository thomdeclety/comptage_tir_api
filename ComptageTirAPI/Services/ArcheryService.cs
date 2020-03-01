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
        private readonly IMongoCollection<Result> _results;

        public ArcheryService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoConnection"));
            var database = client.GetDatabase("comptage_tir");
            _users = database.GetCollection<User>("users");
            _results = database.GetCollection<Result>("results");
        }

        #region Account
        public void AddAccount(string username, string password)
        {
            _users.InsertOne(new User() { UserName = username, Password = password });
        }

        public void DeleteAccount(string username)
        {
            _users.DeleteOne(u => u.UserName == username);
        }

        public string Connect(string username, string password)
        {
            List<User> temp = _users.Find(u => u.UserName == username).ToList();
            if (temp.Count != 1)
            {
                return "The Account doesn't exist";
            }
            else
            {
                if (temp[0].Password != password)
                {
                    return "The password is incorrect";
                }
                else
                {
                    return "OK";
                }
            }
        }
        #endregion

        #region Results
        public void AddResult(Result result)
        {
            var temp = result.Date;

            _results.InsertOne(result);
        }
        
        public List<Result> GetResults(string user, string range, string location, string date)
        {
            List<Result> result = _results.Find(r => r.Owner == user).ToList();
            if (range != "toutes" && range != "none")
            {
                DateTime time = DateTime.Now;
                switch (range)
                {
                    case "7 jours":
                        {
                            result = result.Where(r => time - Convert.ToDateTime(r.Date) <= TimeSpan.FromDays(7)).ToList();
                        }
                        break;
                    case "1 mois":
                        {
                            result = result.Where(r => time - Convert.ToDateTime(r.Date) <= TimeSpan.FromDays(30)).ToList();
                        }
                        break;
                    case "3 mois":
                        {
                            result = result.Where(r => time - Convert.ToDateTime(r.Date) <= TimeSpan.FromDays(90)).ToList();
                        }
                        break;
                }
            }
            if (location != "none")
            {
                result = result.Where(r => r.Location.Contains(location)).ToList();
            }
            if (date != "none")
            {
                date = date.Replace('-', '/');
                result = result.Where(r => r.Date.Contains(date)).ToList();
            }

            return result;
        }

        public Result GetLastResult(string user)
        {
            var temp = _results.Find(r => r.Owner == user).ToList();
            return temp.Count == 0 ? null : temp.Last();
        }

        public void DeleteResult(string id)
        {
            _results.DeleteOne(r => r.Id == id);
        }
        #endregion
    }
}
