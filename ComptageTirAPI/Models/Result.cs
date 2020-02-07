using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComptageTirAPI.Models
{
    public class Result
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public bool Competition { get; set; }
        public List<List<int>> Arrows { get; set; }
    }
}
