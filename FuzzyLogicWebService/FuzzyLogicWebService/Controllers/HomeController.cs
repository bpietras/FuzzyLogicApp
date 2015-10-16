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

        public ActionResult Index(String messageToUser)
        {
            ViewBag.CurrentPage = "home";
            ViewBag.Message = messageToUser;
            return View();
        }

        public ActionResult AboutFuzzyLogic()
        {
            ViewBag.CurrentPage = "aboutFuzzy";
            return View("About");
        }

        public ActionResult AboutPage()
        {
            ViewBag.CurrentPage = "aboutPage";
            return View();
        }

        /*[Authorize]
        public FileContentResult SaveFileLocally(int modelId)
        {
            logger.Info(String.Format("Request for FIS file for model: {0}", modelId));
            FuzzyModel fuzzyModel = repository.GetModelById(modelId);
            FISFileContent content = createFisFileContentObject(fuzzyModel);
            string stringContent = saveFisFileContentToByteArray(content);
            FileContentResult file = new FileContentResult(getBytes(stringContent), "plain/text");
            string fileName = fuzzyModel.Name + ".fis";
            file.FileDownloadName = fileName;
            logger.Info(String.Format("FIS File for model: {0} created with name: {1}: \n{2}", modelId, fileName, stringContent));
            return file;
        }*/

        [Authorize]
        public FileResult SaveFileLocally(int modelId)
        {
            logger.Info(String.Format("Request for FIS file for model: {0}", modelId));
            FuzzyModel fuzzyModel = repository.GetModelById(modelId);
            FISFileContent content = createFisFileContentObject(fuzzyModel);
            string stringContent = saveFisFileContentToByteArray(content);
            string fileName = fuzzyModel.Name + ".fis";
            logger.Info(String.Format("FIS File for model: {0} created with name: {1}: \n{2}", modelId, fileName, stringContent));

            return File(getBytes(stringContent), "plain/text",fileName);
        }

        private string saveFisFileContentToByteArray(FISFileContent content)
        {
            return new FISFileCreator().writeFisFileFromGivenModel(content);
        }

        private FISFileContent createFisFileContentObject(FuzzyModel fuzzyModel)
        {
            return new FisFunctionUtils().mapFuzzyModelToFisFileContent(fuzzyModel);
        }

        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
