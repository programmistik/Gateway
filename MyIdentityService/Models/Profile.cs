using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Models
{
    public class Profile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IdentityId { get; set; }
        public string AppUserId { get; set; }
        public string Name { get; set; }
        public string Avatara { get; set; }
        public string From { get; set; }
        public string Whatsapp { get; set; }
        public string Skype { get; set; }

        public List<string> Friends { get; set; }
    }
}
