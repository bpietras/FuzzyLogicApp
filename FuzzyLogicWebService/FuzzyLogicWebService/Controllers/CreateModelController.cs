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
        EntityFrameworkContext context = new EntityFrameworkContext();

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult BrowseModels()
        {
            string userName = HttpContext.User.Identity.Name;
            ViewBag.UserName = userName;
            int id = (int)Session["userId"];
            return View(rep.GetUserModels(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(FuzzyModel fuzzyModel)
        {
            fuzzyModel.UserID = (int)Session["userId"];
            context.Models.Add(fuzzyModel);
            context.SaveChanges();
            return RedirectToAction("BrowseModels","CreateModel");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int? modelID)
        {
            FuzzyModel modelToDelete = context.Models.Find(modelID);
            context.Models.Remove(modelToDelete);
            context.SaveChanges();
            return RedirectToAction("BrowseModels", "CreateModel");
        }

        public ActionResult ModelDetails(FuzzyModel fuzzyModel)
        {
            //implement
            return RedirectToAction("BrowseModels", "CreateModel");
        }

    }
}
