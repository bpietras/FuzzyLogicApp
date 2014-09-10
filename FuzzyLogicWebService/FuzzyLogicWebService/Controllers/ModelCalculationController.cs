using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyLogicWebService.Controllers
{
    public class ModelCalculationController : HigherController
    {
        //
        // GET: /ModelCalculation/

        [Authorize]
        public ActionResult ModelCalculation()
        {
            return View();
        }

    }
}
