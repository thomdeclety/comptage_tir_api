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
        private readonly IMongoCollection<ArrowSpec> _specs;

        public ArcheryService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoConnection"));
            var database = client.GetDatabase("comptage_tir");
            _users = database.GetCollection<User>("users");
            _results = database.GetCollection<Result>("results");
            _specs = database.GetCollection<ArrowSpec>("specs");
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

        #region Spec

        public void AddSpec(ArrowSpec spec)
        {
            _specs.InsertOne(spec);
        }

        public List<ArrowSpec> GetSpecs(string user)
        {
            return _specs.Find(s => s.Owner == user).ToList();
        }

        public void ModifySpec(ArrowSpec spec)
        {
             spec.Specs = spec.Specs.OrderBy(s => s[0]).ToList();
            _specs.ReplaceOne(s => s.Id == spec.Id, spec);
        }

        public void DeleteSpec(string id)
        {
            _specs.DeleteOne(s => s.Id == id);
        }

        #endregion

        #region Results
        public void AddResult(Result result)
        {
            var temp = result.Date;

            _results.InsertOne(result);
        }
        
        public List<Result> GetResults(string user, string range, string location, string date, bool competition)
        {
            List<Result> result = _results.Find(r => r.Owner == user).ToList();

            if (competition)
            {
                result = result.Where(r => r.Competition).ToList();
            }

            if (range != "toutes" && range != "none")
            {
                DateTime time = DateTime.Now;
                switch (range)
                {
                    case "7jours":
                        {
                            result = result.Where(r => time - DateTime.ParseExact(r.Date, "dd/MM/yyyy", null) <= TimeSpan.FromDays(7)).ToList();
                        }
                        break;
                    case "1mois":
                        {
                            result = result.Where(r => time - DateTime.ParseExact(r.Date, "dd/MM/yyyy", null) <= TimeSpan.FromDays(30)).ToList();
                        }
                        break;
                    case "3mois":
                        {
                            result = result.Where(r => time - DateTime.ParseExact(r.Date, "dd/MM/yyyy", null) <= TimeSpan.FromDays(90)).ToList();
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

            result.Reverse();
            return result;           
        }

        public List<int> GetSummary(List<Result> results)
        {
            if (results.Count < 2)
            {
                if (results.Count == 1)
                {
                    List<int> res = new List<int>();
                    results[0].Arrows.ForEach(r => res.Add(r[0]));

                    return res;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<int> res = new List<int>();

                results.OrderBy(r => r.Arrows.Count);
                results.Reverse();

                for (int i = 0; i < results[0].Arrows.Count; i++)
                {
                    int tempValue = 0;
                    int tempCount = 0;

                    foreach (var result in results)
                    {
                        if (i < result.Arrows.Count)
                        {
                            tempValue += result.Arrows[i].Sum();
                            tempCount++;
                        }
                    }
                    res.Add(tempValue / tempCount);
                }

                return res;
            }
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
