using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicModel;
using FuzzyLogicWebService.FISFiles.FISModel;
using FuzzyLogicWebService.FISFiles;
using FuzzyLogicWebService.Models.Functions;
using FuzzyLogicWebService.Models;

namespace FuzzyLogicWebService.Controllers
{
    public class HomeController : HigherController
    {
        public HomeController(IDatabaseRepository modelRepository)
            : base(modelRepository)
        {
        }

        public ActionResult Index()
        {
            ViewBag.CurrentPage = "home";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.CurrentPage = "about";
            return View();
        }

        [Authorize]
        public FileContentResult SaveFileLocally(int modelId)
        {
            FuzzyModel fuzzyModel = repository.GetModelById(modelId);
            FISFileContent content = new FisFunctionUtils().mapFuzzyModelToFisFileContent(fuzzyModel);
            byte[] contentIntoByteArray = new FISFileCreator().writeFisFileFromGivenModel(content);
            FileContentResult file = new FileContentResult(contentIntoByteArray, "plain/text");
            string fileName = fuzzyModel.Name + ".fis";
            file.FileDownloadName = fileName;
            return file;
        }
    }
}
