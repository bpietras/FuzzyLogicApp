using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles;
using FuzzyLogicWebService.FISFiles.FISModel;

namespace FuzzyLogicWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [ActionName("ShowLinkToSave")]
        public ActionResult SaveFileLocally()
        {
            return View("SaveFileLocally");
        }

        public FileResult SaveFileLocally(string fileContent)
        {
            FISFileCreator fileCreator = new FISFileCreator();

            return File(fileCreator.writeFisFileFromGivenModel(fileContent),System.Net.Mime.MediaTypeNames.Application.Octet);
        }
    }
}
