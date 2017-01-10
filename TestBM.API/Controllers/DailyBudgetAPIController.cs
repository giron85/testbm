using log4net;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TestBM.Data;
using TestBM.Model;

namespace TestBM.API.Controllers
{
    [RoutePrefix("api")]
    public class DailyBudgetAPIController : ApiController
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("dailybudgetremain")]
        public BudgetRemaining Get(string username)
        {
            BudgetRemaining response = new BudgetRemaining();

            try
            {
                MongoDb db = new MongoDb();
                var user = db.Get(username);
                double remainBudget = user.RemainBudget;

                double dailyBudget = remainBudget / (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - (DateTime.Now.Day - 1));
                                
                response.DailyBudget = dailyBudget;

                var costByUser = db.GetCostByUser(username);

                double todayTotalCost = 0;

                foreach (var cost in costByUser)
                {
                    todayTotalCost += cost.CostDetail;
                }

                response.RemainingDailyBudget = dailyBudget - todayTotalCost;

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return response;
            }            
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("insertuser")]
        public void Post(string username, string password, double montlyBudget)
        {
            var userConf = new UserConf
            {
                MontlyBudget = montlyBudget,
                Password = password,
                Username = username,
                RemainBudget = montlyBudget
            };
            MongoDb db = new MongoDb();
            db.PostConf(userConf);
        }
    }
}
