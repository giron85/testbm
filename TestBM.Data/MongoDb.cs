using System;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using TestBM.Model;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

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
            var collection = _database.GetCollection<UserConf>("UserConf");
            
            var result = collection.Find(x => x.Username.Equals(username)).FirstOrDefault();

            return result;
        }

        public bool PostCost(string username, double costImport, string costInfo)
        {
            try
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

                var collectionUserConf = _database.GetCollection<UserConf>("UserConf");
                var user = collectionUserConf.Find(x => x.Username.Equals(username)).FirstOrDefault();
                user.RemainBudget = user.RemainBudget - costImport;

                var builder = Builders<UserConf>.Filter;
                var filter = builder.Eq("Username", username);
                var update = Builders<UserConf>.Update.Set(x => x.RemainBudget, user.RemainBudget);

                var updateResult = collectionUserConf.UpdateOne(filter, update);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public void PostConf(UserConf userConf)
        {
            if (_database == null) GetMongoDatabase();
            var collection = _database.GetCollection<UserConf>("UserConf");
            
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

        public SignInStatus Login(string email, string password, bool rememberMe, bool shouldLockout)
        {
            if (_database == null) GetMongoDatabase();
            var collection = _database.GetCollection<UserConf>("UserConf");

            var result = collection.Find(x => x.Username.Equals(email) && x.Password.Equals(password)).ToList();
            
            if (result.Any()) return SignInStatus.Success;
            else return SignInStatus.Failure;
        }

        public SignInStatus Register(string user, string password)
        {
            try
            {
                if (_database == null) GetMongoDatabase();
                var collection = _database.GetCollection<UserConf>("UserConf");

                var userConf = new UserConf();
                userConf.Username = user;
                userConf.Password = password;

                collection.InsertOne(userConf);
                        
                return SignInStatus.Success;
            }
            catch (Exception ex)
            {
                return SignInStatus.Failure;
            }            
        }

        public bool ModifyBudget(double montlyBudget, string userName)
        {
            try
            {
                if (_database == null) GetMongoDatabase();
                var collection = _database.GetCollection<UserConf>("UserConf");
                
                
                var user = collection.Find(x => x.Username.Equals(userName)).FirstOrDefault();

                if (user.RemainBudget != 0)
                {
                    var consumed = user.MontlyBudget - user.RemainBudget;
                    user.RemainBudget = montlyBudget - consumed;
                }
                else
                {
                    user.RemainBudget = montlyBudget;
                }
                user.MontlyBudget = montlyBudget;

                var builder = Builders<UserConf>.Filter;
                var filter = builder.Eq("Username", userName);
                var update = Builders<UserConf>.Update.Set(x => x.RemainBudget, user.RemainBudget).Set(x => x.MontlyBudget, user.MontlyBudget);
                var updateResult = collection.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
    }

}
