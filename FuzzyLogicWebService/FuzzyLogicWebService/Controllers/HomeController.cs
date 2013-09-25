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
            FISFileCreator fisCreator = new FISFileCreator();
            FISFileContent contentOfFile = new FISFileContent();
            FISSystem systemProperties = new FISSystem();
            systemProperties.Name = "Tips model";
            systemProperties.InputsNumber = 4;
            systemProperties.OutputsNumber = 1;
            systemProperties.RulesNumber = 9;
            systemProperties.AndMethod = systemProperties.AggMethod = systemProperties.ImpMethod = systemProperties.OrMethod = "ni chuja nie ma";
            systemProperties.DefuzzMethod = "totalnie abstrakcyjna";
            contentOfFile.SystemProperties = systemProperties;
            fisCreator.writeFisFileFromGivenModel(contentOfFile);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
