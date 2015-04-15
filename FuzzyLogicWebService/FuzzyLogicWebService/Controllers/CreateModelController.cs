using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.DataVisualization.Charting;
using FuzzyLogicWebService.Models;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models.ViewModels;
using FuzzyLogicWebService.Models.Functions;
using FuzzyLogicWebService.Helpers;

namespace FuzzyLogicWebService.Controllers
{
    public class CreateModelController : HigherController
    {
        ModelsRepository rep = new ModelsRepository();
        
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CurrentPage = "create";
            return View();
        }

        [Authorize]
        public ActionResult BrowseModels()
        {
            ViewBag.CurrentPage = "browse";
            ViewBag.UserName = HttpContext.User.Identity.Name;
            int id = GetUserCookieValue();
            return View("BrowseModels",rep.GetUserModels(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(FuzzyModel fuzzyModel)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            if (ModelState.IsValid)
            {
                int id = GetUserCookieValue();
                fuzzyModel.OutputsNumber = 1;
                try
                {
                    fuzzyModel = rep.AddModelForUser(id, fuzzyModel);
                
                AddModelIdToSession(fuzzyModel.ModelID);
                return RedirectToAction("AddInputVariables", new { modelId = fuzzyModel.ModelID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Model with that name already exists." + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Error while creating model");
            }
            return View();
        }


        [Authorize]
        public ActionResult AddInputVariables(int modelId)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            FuzzyModel fuzzyModel = rep.GetModelById(modelId);
            List<FuzzyVariable> inputs = new List<FuzzyVariable>();
            for (int w = 0; w < fuzzyModel.InputsNumber; w++)
            {
                inputs.Add(FuzzyLogicModel.FuzzyVariable.CreateFuzzyVariable(FuzzyLogicService.InputVariable));
            }
            return View("AddInputVariables", inputs);
        }
        

        [Authorize]
        [HttpPost]
        public ActionResult AddInputVariables(IEnumerable<FuzzyVariable> listOfInVariables)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            if (ModelState.IsValid)
            {
                try
                {
                    FuzzyModel fuzzyModel = rep.GetModelById(GetCurrentModelId());
                    rep.AddInputVariableForModel(fuzzyModel.ModelID, listOfInVariables);
                    rep.UpdateModelStatus(fuzzyModel, 1);
                    return RedirectToActionPermanent("AddOutputVariables", new { modelId = fuzzyModel.ModelID });
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "Error while creating inputs");
                }
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są niepoprawne");
            }
            return View("AddInputVariables", listOfInVariables);
        }

        [Authorize]
        public ActionResult AddOutputVariables(int modelId)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            FuzzyModel fuzzyModel = rep.GetModelById(modelId);
            List<FuzzyVariable> outputs = new List<FuzzyVariable>();
            for (int w = 0; w < fuzzyModel.OutputsNumber; w++)
            {
                outputs.Add(FuzzyLogicModel.FuzzyVariable.CreateFuzzyVariable(FuzzyLogicService.OutputVariable));
            }
            return View("AddOutputVariables", outputs);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOutputVariables(IEnumerable<FuzzyVariable> listOfOutVariables)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            if (ModelState.IsValid)
            {
                try
                {
                    int modelId = GetCurrentModelId();
                    rep.AddOutputVariableForModel(modelId, listOfOutVariables);
                    rep.UpdateModelStatus(modelId, 2);
                    return RedirectToAction("AddFunctionsForVariables");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Nie można zapisać do bazy");
                }
            }
            else
            {
                ModelState.AddModelError("", "Podane dane są niepoprawne");
            }
            return View("AddOutputVariables", listOfOutVariables);
        }

        [Authorize]
        public ActionResult AddFunctionsForVariables()
        {
            ViewBag.CurrentPage = "create";
            IEnumerable<FuzzyVariable> allVariables = rep.GetVariablesForModel(GetCurrentModelId());
            return View("AddFunctionsForVariables", allVariables);
        }

        [Authorize]
        public ActionResult DisplayMembershipFuncDetails(int currentVarId)
        {
            ViewBag.CurrentPage = "create";
            AddVariableIdToSession(currentVarId);
            FuzzyVariable currentVariable = rep.GetVariableById(currentVarId);
            ViewBag.VariableMinValue = currentVariable.MinValue;
            ViewBag.VariableMaxValue = currentVariable.MaxValue;
            ViewBag.IsEdit = false;
            if (currentVariable.MembershipFunctions.Count == 0)
            {
                List<MembershipFunction> functions = new List<MembershipFunction>(currentVariable.NumberOfMembFunc);
                for (int w = 0; w < currentVariable.NumberOfMembFunc; w++)
                {
                    functions.Add(new MembershipFunction());
                }
                return View("AddMembershipFuncDetails", functions);
            }
            else
            {
                return View("AddMembershipFuncDetails", currentVariable.MembershipFunctions);
            }
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddMembershipFuncDetails(List<MembershipFunction> listOfMfs)
        {
            ViewBag.CurrentPage = "create";
            FuzzyVariable currentVariable = rep.GetVariableById(GetCurrentVariableId());
            if (ModelState.IsValid && ValidateMemmbershipFunctions(new List<FuzzyVariable>(){currentVariable}))
            {
                //validate the range
                listOfMfs = rep.AddMembFuncForVariable(GetCurrentVariableId(), listOfMfs);
                return RedirectToAction("AddFunctionsForVariables");
            }
            else
            {
                return View("AddMembershipFuncDetails", listOfMfs);

            }
        }


        [Authorize] //update
        public ActionResult AddRulesToModel()
        {
            ViewBag.CurrentPage = "create";
            FuzzyModel fuzzyModel = rep.GetModelById(GetCurrentModelId());
            int moveForward = 0;
            //czy sa obecne wszystkie zmienne
            foreach (FuzzyVariable variable in fuzzyModel.FuzzyVariables)
            {
                if (variable.NumberOfMembFunc == variable.MembershipFunctions.Count)
                {
                    moveForward++;
                }
            }
            if (moveForward == fuzzyModel.FuzzyVariables.Count)
            {
                rep.UpdateModelStatus(fuzzyModel, 3);
                string basicRule = CreateRuleContent(fuzzyModel);
                List<FuzzyRule> rules = new List<FuzzyRule>();
                for (int w = 0; w < fuzzyModel.RulesNumber; w++)
                {
                    FuzzyRule rule = new FuzzyRule();
                    rule.StringRuleContent = basicRule;
                    rule.FISRuleContent = "empty";
                    rules.Add(rule);
                }
                return View(rules);
            }
            else
            {
                IEnumerable<FuzzyVariable> allVariables = rep.GetVariablesForModel(GetCurrentModelId());
                return View("AddFunctionsForVariables", allVariables);
            }
        }

        private String CreateRuleContent(FuzzyModel fuzzyModel)
        {
            string inputs = "";
            string outputs = "";
            foreach (FuzzyVariable variable in fuzzyModel.FuzzyVariables)
            {
                string value = variable.MembershipFunctions.ElementAt(0) != null ? variable.MembershipFunctions.ElementAt(0).Name : "";
                string predicate = String.Format("({0} is {1}) ", variable.Name, value);
                if (variable.VariableType == FuzzyLogicService.InputVariable)
                {
                    string connection = inputs.Count() > 0 ? " and " : "";
                    inputs = inputs + connection + predicate;
                }

                if (variable.VariableType == FuzzyLogicService.OutputVariable)
                {
                    string connection = outputs.Count() > 0 ? " and " : "";
                    outputs = outputs + connection + predicate;
                }

            }

            string ruleContent = String.Format("if {0} then {1}", inputs, outputs);
            return ruleContent;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddRulesToModel(IEnumerable<FuzzyRule> rules)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                FuzzyModel fuzzyModel = rep.GetModelById(GetCurrentModelId());
                RulesParserUtility parser = new RulesParserUtility();
                try
                {
                    rules = parser.ParseStringRules(rules, fuzzyModel);
                }
                catch (Exception parserExc)
                {
                    ViewBag.ParserErrormessage = parserExc.Message;
                    return View("AddRulesToModel", rules);
                }
                rep.AddRulesToModel(fuzzyModel.ModelID, rules);
                rep.UpdateModelStatus(fuzzyModel, 4);
                return RedirectToAction("ModelDetails", new { modelId = fuzzyModel.ModelID });
            }
            else
            {
                return View("AddRulesToModel", rules);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int modelID)
        {
            ViewBag.CurrentPage = "browse";
            rep.DeleteModelById(modelID);
            return RedirectToAction("BrowseModels", "CreateModel");
        }

        [Authorize]
        public ActionResult ModelDetails(int modelId)
        {
            ViewBag.CurrentPage = "browse";
            ViewBag.IsEdit = false;
            FuzzyModel model = rep.GetModelById(modelId);
            return View("ModelDetails", model);
        }

        [Authorize]
        public ActionResult EditModel(int modelId)
        {
            ViewBag.CurrentPage = "browse";
            FuzzyModel modelObj = rep.GetModelById(modelId);
            ViewBag.VariableWidth = (int)100/modelObj.FuzzyVariables.Count;
            ViewBag.IsEdit = true;
            return View("EditModel", modelObj);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditModel(FuzzyModel model)
        {
            ViewBag.CurrentPage = "browse";
            RulesParserUtility parser = new RulesParserUtility();
            try
            {
                parser.ParseStringRules(model.FuzzyRules, model);
            }
            catch (Exception parserExc)
            {
                ViewBag.ParserErrormessage = parserExc.Message;
                return RedirectToAction("EditModel", model.ModelID);
            }
            if (ValidateModel(model))
            {
                rep.SaveEditedModel(model);
            }
            else
            {
                return View("EditModel", model);
            }
            return View("ModelDetails", model);
        }

        [Authorize]
        public ActionResult GoBackWhereStopped(int actionStoppedNumber, int modelId)
        {
            ViewBag.CurrentPage = "create";
            AddModelIdToSession(modelId);
            try
            {
                return GetLastCheckpoint(actionStoppedNumber, modelId);
            }
            catch
            {
                return RedirectToAction("BrowseModels");
            }
        }

        private ActionResult GetLastCheckpoint(int actionStoppedNumber, int modelID)
        {
            string actionName = null;
            FuzzyLogicService.CreateModelActionList.TryGetValue(actionStoppedNumber, out actionName);
            switch(actionStoppedNumber){
                case 0:
                case 1:
                    return RedirectToAction(actionName, new { modelId = modelID });
                case 2:
                case 3:
                    return RedirectToAction(actionName);
                default:
                    throw new Exception("Unknown checkpoint number");
            }
        }

        [Authorize]
        public ActionResult CopyModel(int modelId)
        {
            rep.CopyGivenModel(modelId, GetUserCookieValue());
            return RedirectToAction("BrowseModels");
        }

    }
}
