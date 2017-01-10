using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBM.UI.Controllers
{    
    public class DailyBudgetController : Controller
    {
        // GET: ChartIndex
        //[Authorize]
        public ActionResult ChartIndex()
        {
            return View();
        }

        // GET: AnalyzeCost
        //[Authorize]
        public ActionResult AnalyzeCost()
        {
            return View();
        }
    }
}