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
            ViewBag.CurrentPage = "create";
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
            ViewBag.CurrentPage = "browse";
            ViewBag.UserName = HttpContext.User.Identity.Name;
            int id = (int)Session["userId"];
            return View(rep.GetUserModels(id));
        }

        [Authorize]
        [HttpPost]
        public ViewResult Create(FuzzyModel fuzzyModel)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                int modelId = rep.AddModelForUser((int)Session["userId"], fuzzyModel);
                Session["modelID"] = modelId;
                fuzzyModel.ModelID = modelId;
                List<FVariable> inputs = new List<FVariable>();
                for (int w = 0; w < fuzzyModel.InputsNumber; w++)
                {
                    inputs.Add(new FVariable());
                }
                return View("AddInputVariables", inputs);
            }
            else
            {
                return View("Create", fuzzyModel.ModelID);
            }
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddInputVariables(IEnumerable<FVariable> listOfInVariables)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                rep.AddInputVariableForModel((int)Session["modelID"], listOfInVariables);
                return RedirectToAction("AddOutputVariables");
            }
            else
            {
                return View("AddInputVariables", listOfInVariables);
            }
        }

        [Authorize]
        public ActionResult AddOutputVariables()
        {
            ViewBag.CurrentPage = "create";
            FuzzyModel fuzzyModel = rep.GetModelById((int)Session["modelId"]);
            List<FVariable> outputs = new List<FVariable>();
            for (int w = 0; w < fuzzyModel.OutputsNumber; w++)
            {
                outputs.Add(new FVariable());
            }
            return View(outputs);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOutputVariables(IEnumerable<FVariable> listOfOutVariables)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                rep.AddOutputVariableForModel((int)Session["modelID"], listOfOutVariables);
                return RedirectToAction("BrowseModels");
            }
            else
            {
                return View("AddOutputVariables", listOfOutVariables);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int? modelID)
        {
            ViewBag.CurrentPage = "browse";
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
