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
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService.Controllers
{
    public class HomeController : HigherController
    {
        public HomeController(IDatabaseRepository modelRepository, ILogger appLogger)
            : base(modelRepository, appLogger)
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
            logger.Info(String.Format("Create and save file for model: {0}", modelId));
            FuzzyModel fuzzyModel = repository.GetModelById(modelId);
            FISFileContent content = createFisFileContentObject(fuzzyModel);
            byte[] contentIntoByteArray = saveFisFileContentToByteArray(content);
            FileContentResult file = new FileContentResult(contentIntoByteArray, "plain/text");
            string fileName = fuzzyModel.Name + ".fis";
            file.FileDownloadName = fileName;
            logger.Info(String.Format("File for model: {0} created with name: {1}", modelId, fileName));
            return file;
        }

        private byte[] saveFisFileContentToByteArray(FISFileContent content)
        {
            return new FISFileCreator().writeFisFileFromGivenModel(content);
        }

        private FISFileContent createFisFileContentObject(FuzzyModel fuzzyModel)
        {
            return new FisFunctionUtils().mapFuzzyModelToFisFileContent(fuzzyModel);
        }
    }
}
