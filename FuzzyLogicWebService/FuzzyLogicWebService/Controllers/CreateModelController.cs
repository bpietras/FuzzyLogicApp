using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.DataVisualization.Charting;
using FuzzyLogicWebService.Models;
using FuzzyLogicModel;

namespace FuzzyLogicWebService.Controllers
{
    public class CreateModelController : HigherController
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
            //int id = (int)Session["userId"];
            int id = int.Parse(Request.Cookies.Get(CURRENT_USER_ID).Value);
            return View(rep.GetUserModels(id));
        }

        [Authorize]
        [HttpPost]
        public ViewResult Create(FuzzyModel fuzzyModel)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                int id = int.Parse(GetCookieValue(CURRENT_USER_ID));
                int modelId = rep.AddModelForUser(id, fuzzyModel);
                CreateCookie(CURRENT_MODEL_ID, modelId);
                fuzzyModel.ModelID = modelId;
                List<FuzzyVariable> inputs = new List<FuzzyVariable>();
                for (int w = 0; w < fuzzyModel.InputsNumber; w++)
                {
                    inputs.Add(new FuzzyVariable());
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
        public ActionResult AddInputVariables(IEnumerable<FuzzyVariable> listOfInVariables)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                int modelId = int.Parse(GetCookieValue(CURRENT_MODEL_ID));
                rep.AddInputVariableForModel(modelId, listOfInVariables);
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
           
            //Session["currentVariableId"] = currentVarId;
            CreateCookie(CURRENT_VARIABLE_ID,currentVarId);
            FuzzyVariable currentVariable = rep.GetVariableById(currentVarId);
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
                rep.AddMembFuncForVariable(int.Parse(GetCookieValue(CURRENT_VARIABLE_ID)), listOfMfs);
                IEnumerable<FuzzyVariable> allVariables = rep.GetVariablesForModel(int.Parse(GetCookieValue(CURRENT_MODEL_ID)));
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
            FuzzyModel fuzzyModel = rep.GetModelById(int.Parse(GetCookieValue(CURRENT_MODEL_ID)));
            /*int allVariables = fuzzyModel.InputsNumber + fuzzyModel.OutputsNumber;
            if (fuzzyModel.Variables.Count() == allVariables)
            {
                return RedirectToAction("BrowseModels");
            }
            else
            {*/
            List<FuzzyVariable> outputs = new List<FuzzyVariable>();
                for (int w = 0; w < fuzzyModel.OutputsNumber; w++)
                {
                    outputs.Add(new FuzzyVariable());
                }
                return View(outputs);
            //}
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOutputVariables(IEnumerable<FuzzyVariable> listOfOutVariables)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                int modelId = int.Parse(GetCookieValue(CURRENT_MODEL_ID));
                rep.AddOutputVariableForModel(modelId, listOfOutVariables);
                IEnumerable<FuzzyVariable> allVariables = rep.GetVariablesForModel(modelId);
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
            FuzzyModel fuzzyModel = rep.GetModelById((int)Session["modelId"]);
            List<FuzzyRule> rules = new List<FuzzyRule>();
            for (int w = 0; w < fuzzyModel.RulesNumber; w++)
            {
                rules.Add(new FuzzyRule());
            }
            return View(rules);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddRulesToModel(IEnumerable<FuzzyRule> rules)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                int modelId = int.Parse(GetCookieValue(CURRENT_MODEL_ID));
                rep.AddRulesToModel(modelId, rules);
                return RedirectToAction("ModelDetails", modelId);
            }
            else
            {
                return View("AddRulesToModel", rules);
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
        public ActionResult ModelDetails(int? modelId)
        {
            ViewBag.CurrentPage = "browse";
            FuzzyModel model = rep.GetModelById(modelId);
            //implement
            //return View(model);
            return View("ModelDetails", model);
        }


    }
}
