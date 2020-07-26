using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComptageTirAPI.Models
{
    public class ArrowSpec
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public List<List<float>> Specs { get; set; }
    }
}
