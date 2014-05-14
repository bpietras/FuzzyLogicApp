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
        ModelsRepository rep = new ModelsRepository();
        EntityFrameworkContext context = new EntityFrameworkContext();

        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult BrowseModels()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            return View(rep.Users.ToList().ElementAt(0));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(FuzzyModel fuzzyModel)
        {
            var userId = from u in context.Users
                         where u.Name == HttpContext.User.Identity.Name
                         select u.UserID;
            fuzzyModel.UserID = userId.ToList().ElementAt(0);
            context.Models.Add(fuzzyModel);
            context.SaveChanges();
            return RedirectToAction("BrowseModels","CreateModel");
        }
    }
}
