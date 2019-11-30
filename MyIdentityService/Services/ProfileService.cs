using MongoDB.Driver;
using MyIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Services
{
    public class ProfileService
    {
        private readonly IMongoCollection<Profile> _profile;

        public ProfileService(IInstaDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _profile = database.GetCollection<Profile>(settings.ProfilesCollectionName);
        }

        public List<Profile> Get() =>
            _profile.Find(prof => true).ToList();

        public Profile Get(string id) =>
            _profile.Find<Profile>(prof => prof.AppUserId == id).FirstOrDefault();

        public Profile Create(Profile prof)
        {
            _profile.InsertOne(prof);
            return prof;
        }

        public void Update(string id, Profile profIn) =>
            _profile.ReplaceOne(prof => prof.Id == id, profIn);

        public void Remove(Profile profIn) =>
            _profile.DeleteOne(prof => prof.AppUserId == profIn.Id);

        public void Remove(string id) =>
            _profile.DeleteOne(prof => prof.AppUserId == id);
    }
}

