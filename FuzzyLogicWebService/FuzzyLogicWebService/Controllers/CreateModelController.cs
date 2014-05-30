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

        [Authorize]
        public ActionResult Create(int? modelId)
        {
            if (modelId != null)
            {
                FuzzyModel currentModel = rep.GetModelById(modelId);
                rep.DeleteModelById(modelId);
                return View("Create", currentModel);
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult BrowseModels()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            int id = (int)Session["userId"];
            return View(rep.GetUserModels(id));
        }

        [Authorize]
        [HttpPost]
        public ViewResult Create(FuzzyModel fuzzyModel)
        {
            if (ModelState.IsValid)
            {
                int modelId = rep.AddModelForUser((int)Session["userId"], fuzzyModel);
                Session["modelID"] = modelId;
                fuzzyModel.ModelID = modelId;
                List<InVariable> inputs = new List<InVariable>(fuzzyModel.InputsNumber);
                for (int w = 0; w < fuzzyModel.InputsNumber; w++)
                {
                    inputs.Add(new InVariable());
                }
                fuzzyModel.InputVariables = inputs;
                return View("AddVariables", fuzzyModel);
            }
            else
            {
                return View("Create", fuzzyModel.ModelID);
            }
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddInputVariables(FuzzyModel fuzzyModel)
        {
            if (ModelState.IsValid)
            {
                //add variable
                rep.AddVariableForModel((int)Session["modelID"], fuzzyModel.InputVariables);
                return RedirectToAction("BrowseModels");
            }
            else
            {
                return PartialView("AddVariables");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int? modelID)
        {
            rep.DeleteModelById(modelID);
            return RedirectToAction("BrowseModels", "CreateModel");
        }

        public ActionResult ModelDetails(FuzzyModel fuzzyModel)
        {
            //implement
            return RedirectToAction("BrowseModels", "CreateModel");
        }

    }
}
