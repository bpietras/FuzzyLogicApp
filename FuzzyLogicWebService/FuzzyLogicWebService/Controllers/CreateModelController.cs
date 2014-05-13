using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles.FISModel;
using FuzzyLogicWebService.FISFiles.DBModel;

namespace FuzzyLogicWebService.Controllers
{
    public class CreateModelController : Controller
    {
        ModelsRepository rep = new ModelsRepository();
        // GET: /CreateModel/

        public ActionResult Create()
        {
            return View(rep.Models);
        }

    }
}
