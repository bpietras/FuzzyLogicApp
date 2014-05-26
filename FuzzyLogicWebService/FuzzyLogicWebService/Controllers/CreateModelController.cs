using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles.FISModel;
using FuzzyLogicWebService.FISFiles.DBModel;
using System.Web.Security;
using System.Web.UI.DataVisualization.Charting;
using FuzzyLogicWebService.FISFiles;

namespace FuzzyLogicWebService.Controllers
{
    public class CreateModelController : Controller
    {
        private ModelsRepository rep = new ModelsRepository();

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult BrowseModels()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            int id = (int)Session["userId"];
            return View(rep.GetUserModels(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(FuzzyModel fuzzyModel)
        {
            rep.AddModelForUser((int)Session["userId"], fuzzyModel);
            return View("_AddVariables");
            //return RedirectToAction("AddVariables", "CreateModel", new {number = fuzzyModel.InputsNumber });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int? modelID)
        {
            rep.DeleteModelById(modelID);
            return RedirectToAction("BrowseModels", "CreateModel");
        }

        public ActionResult ModelDetails(FuzzyModel fuzzyModel)
        {
            //implement
            return RedirectToAction("BrowseModels", "CreateModel");
        }

    }
}
