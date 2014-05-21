using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles;
using FuzzyLogicWebService.FISFiles.FISModel;
using FuzzyLogicWebService.FISFiles.DBModel;

namespace FuzzyLogicWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";
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

        [Authorize]
        public FileContentResult SaveFileLocally(FuzzyModel fuzzyModel)
        {
            FISSystem systemProperties = new FISSystem();
            systemProperties.Name = fuzzyModel.Name;
            systemProperties.InputsNumber = fuzzyModel.InputsNumber;
            systemProperties.OutputsNumber = fuzzyModel.OutputsNumber;
            systemProperties.RulesNumber = fuzzyModel.RulesNumber;

            FISFileContent content = new FISFileContent();
            content.SystemProperties = systemProperties;
            //todo: implement some other way
            byte[] contentIntoByteArray = new FISFileCreator().writeFisFileFromGivenModel(content);
            FileContentResult file = new FileContentResult(contentIntoByteArray, "plain/text");
            string fileName = fuzzyModel.Name + ".fis";
            file.FileDownloadName = fileName;
            return file;
        }
    }
}
