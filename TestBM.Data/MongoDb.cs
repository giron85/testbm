using System;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using TestBM.Model;
using System.Collections.Generic;

namespace TestBM.Data
{
    public class MongoDb
    {
        private IMongoDatabase _database;

        public IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["mongoServerUrl"].ToString());
            _database = client.GetDatabase(ConfigurationManager.AppSettings["MongoDBDatabaseName"]);
            return _database;
        }

        public UserConf Get(string username)
        {
            if (_database == null) GetMongoDatabase();
            var collection = _database.GetCollection<UserConf>("UserConfig");
            
            var result = collection.Find(x => x.Username.Equals(username)).FirstOrDefault();

            return result;
        }

        public void PostCost(string username, double costImport, string costInfo)
        {
            if (_database == null) GetMongoDatabase();
            var collection = _database.GetCollection<Cost>("Cost");

            var cost = new Cost
            {
                CostDateTime = DateTime.Now,
                CostDetail = costImport,
                Username = username,
                CostInfo = costInfo
            };

            collection.InsertOne(cost);

            var collectionUserConf = _database.GetCollection<UserConf>("UserConfig");
            var result = collectionUserConf.Find(x => x.Username.Equals(username)).FirstOrDefault();
            result.RemainBudget = result.RemainBudget - costImport;
            var filter = Builders<UserConf>.Filter.Empty;
            collectionUserConf.ReplaceOne(filter, result);
        }

        public void PostConf(UserConf userConf)
        {
            if (_database == null) GetMongoDatabase();
            var collection = _database.GetCollection<UserConf>("UserConfig");
            
            collection.InsertOne(userConf);
        }

        public List<Cost> GetCostByUser(string username)
        {
            if (_database == null) GetMongoDatabase();
            var collection = _database.GetCollection<Cost>("Cost");            

            var result = collection.Find(x => x.Username.Equals(username)).ToList();
            List<Cost> todayCostByUser = new List<Cost>();
            foreach (var item in result)
            {
                var dayOfYear = item.CostDateTime.DayOfYear;
                if (dayOfYear == DateTime.Now.DayOfYear)
                    todayCostByUser.Add(item);
            }

            return todayCostByUser;
        }
    }

}
