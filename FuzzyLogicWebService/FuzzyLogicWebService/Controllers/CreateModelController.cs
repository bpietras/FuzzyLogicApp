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
            User user = rep.Users.Where(x=>x.Name == userName).First();
            ViewBag.UserName = userName;
            return View(rep.GetUserModels(user.UserID));
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

        [HttpPost]
        public ActionResult Delete(FuzzyModel fuzzyModel)
        {
            //implement
            return RedirectToAction("BrowseModels", "CreateModel");
        }

        public ActionResult ModelDetails(FuzzyModel fuzzyModel)
        {
            //implement
            return RedirectToAction("BrowseModels", "CreateModel");
        }

    }
}
