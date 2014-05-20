using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles.FISModel;
using FuzzyLogicWebService.FISFiles.DBModel;
using System.Web.Security;

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
            return View(rep.GetUserModels((int)Session["userId"]));
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

        [HttpPost]
        public ActionResult Delete(int? modelID)
        {
            FuzzyModel modelToDelete = context.Models.Where(x => x.ModelID == modelID).First();
            //implement
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
