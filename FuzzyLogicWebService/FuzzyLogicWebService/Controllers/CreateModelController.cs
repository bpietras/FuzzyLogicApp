using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.FISFiles.DBModel;
using System.Web.Security;
using System.Web.UI.DataVisualization.Charting;

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
                FuzzyModel currentModel = rep.GetModelById(modelId, false);
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
                //IEnumerable<FVariable> inputs = 
                //return View("AddFunctionsForVariables",inputs);
                return RedirectToAction("AddOutputVariables");
            }
            else
            {
                return View("AddInputVariables", listOfInVariables);
            }
        }

        [Authorize]
        public ActionResult DisplayMembershipFuncDetails(int currentVarId)
        {
            ViewBag.CurrentPage = "create";
           
                Session["currentVariableId"] = currentVarId;
                FVariable currentVariable = rep.GetVariableById(currentVarId);
                List<MembershipFunction> functions = new List<MembershipFunction>(currentVariable.NumberOfMembFunc);
                for (int w = 0; w < currentVariable.NumberOfMembFunc; w++)
                {
                    functions.Add(new MembershipFunction());
                }
                return View("AddMembershipFuncDetails", functions);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddMembershipFuncDetails(IEnumerable<MembershipFunction> listOfMfs)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                rep.AddMembFuncForVariable((int)Session["currentVariableId"], listOfMfs);
                IEnumerable<FVariable> allVariables = rep.GetVariablesForModel((int)Session["modelID"], false);
                return View("AddFunctionsForVariables", allVariables);
            }
            else
            {
                return View("AddMembershipFuncDetails", listOfMfs);

            }
        }

        [Authorize]
        public ActionResult AddOutputVariables()
        {
            ViewBag.CurrentPage = "create";
            FuzzyModel fuzzyModel = rep.GetModelById((int)Session["modelId"], false);
            /*int allVariables = fuzzyModel.InputsNumber + fuzzyModel.OutputsNumber;
            if (fuzzyModel.Variables.Count() == allVariables)
            {
                return RedirectToAction("BrowseModels");
            }
            else
            {*/
                List<FVariable> outputs = new List<FVariable>();
                for (int w = 0; w < fuzzyModel.OutputsNumber; w++)
                {
                    outputs.Add(new FVariable());
                }
                return View(outputs);
            //}
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOutputVariables(IEnumerable<FVariable> listOfOutVariables)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                rep.AddOutputVariableForModel((int)Session["modelID"], listOfOutVariables);
                IEnumerable<FVariable> allVariables = rep.GetVariablesForModel((int)Session["modelID"], false);
                return View("AddFunctionsForVariables", allVariables);
                //return RedirectToAction("BrowseModels");
            }
            else
            {
                return View("AddOutputVariables", listOfOutVariables);
            }
        }

        [Authorize]
        public ActionResult AddRulesToModel()
        {
            ViewBag.CurrentPage = "create";
            FuzzyModel fuzzyModel = rep.GetModelById((int)Session["modelId"], false);
            List<Rule> rules = new List<Rule>();
            for (int w = 0; w < fuzzyModel.RulesNumber; w++)
            {
                rules.Add(new Rule());
            }
            return View(rules);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddRulesToModel(IEnumerable<Rule> rules)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                rep.AddRulesToModel((int)Session["modelId"], rules);
                return RedirectToAction("BrowseModels");
            }
            else
            {
                return View("AddRuesToModel", rules);
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

        [Authorize]
        public ActionResult ModelDetails(FuzzyModel fuzzyModel)
        {
            //FuzzyModel model = rep.GetModelById((int)Session["modelId"], true);
            //implement
            //return View(model);
            return RedirectToAction("BrowseModels", "CreateModel");
        }


    }
}
