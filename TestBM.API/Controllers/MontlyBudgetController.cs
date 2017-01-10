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
    public class MontlyBudgetController : ApiController
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("montlybudgetremain")]
        public MontlyBudgetRemaining Get(string username)
        {
            MontlyBudgetRemaining response = new MontlyBudgetRemaining();

            try
            {
                MongoDb db = new MongoDb();
                var user = db.Get(username);
                response.MontlyBudget = user.MontlyBudget;
                response.MontlyBudgetRemain = user.RemainBudget;

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return response;
            }
        }
    }
}
