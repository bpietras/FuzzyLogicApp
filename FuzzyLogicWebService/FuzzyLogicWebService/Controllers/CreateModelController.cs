using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles.FISModel;

namespace FuzzyLogicWebService.Controllers
{
    public class CreateModelController : Controller
    {
        //
        // GET: /CreateModel/

        public ActionResult CheckFirstTab(FISSystem system)
        {
            return View("FirstTabReview", system);
        }

    }
}
