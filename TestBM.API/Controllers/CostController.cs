using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TestBM.Data;
using TestBM.Model;

namespace TestBM.API.Controllers
{
    [RoutePrefix("api")]
    public class CostController : ApiController
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("insertcost")]
        public string Post(string username, double costImport, string costInfo)
        {
            try
            {
                MongoDb db = new MongoDb();
                db.PostCost(username, costImport, costInfo);

                return "OK";
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return "KO";
            }            
        }

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("getcost")]
        public List<Cost> Get(string username)
        {
            List<Cost> costs = new List<Cost>();

            try
            {
                MongoDb db = new MongoDb();
                costs = db.GetCostByUser(username);

                return costs;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return costs;
            }
        }
    }
}
