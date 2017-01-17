using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace TestBM.Model
{
    public class UserConf
    {
        public Guid id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double MontlyBudget { get; set; }
        public double RemainBudget { get; set; }
    }
}
