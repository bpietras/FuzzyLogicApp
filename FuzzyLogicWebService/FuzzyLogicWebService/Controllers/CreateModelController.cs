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
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService.Controllers
{
    public class CreateModelController : HigherController
    {
        public CreateModelController(IDatabaseRepository modelRepository, ILogger appLogger)
            : base(modelRepository, appLogger)
        {
        }

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
            return View("BrowseModels",repository.GetUserModels(id));
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
                    fuzzyModel = repository.AddModelForUser(id, fuzzyModel);
                
                AddModelIdToSession(fuzzyModel.ModelID);
                return RedirectToAction("AddInputVariablesByModel", fuzzyModel);
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
            FuzzyModel fuzzyModel = repository.GetModelById(modelId);
            return RedirectToAction("AddInputVariablesByModel", fuzzyModel);
        }

        [Authorize]
        public ActionResult AddInputVariablesByModel(FuzzyModel fuzzyModel)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
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
                    FuzzyModel fuzzyModel = repository.GetModelById(GetCurrentModelId());
                    repository.AddInputVariableForModel(fuzzyModel.ModelID, listOfInVariables);
                    repository.UpdateModelStatus(fuzzyModel, 1);
                    return RedirectToAction("AddOutputVariablesByModel", fuzzyModel);
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
            FuzzyModel fuzzyModel = repository.GetModelById(modelId);
            return RedirectToAction("AddOutputVariablesByModel", fuzzyModel);
        }

        [Authorize]
        public ActionResult AddOutputVariablesByModel(FuzzyModel fuzzyModel)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
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
                    repository.AddOutputVariableForModel(modelId, listOfOutVariables);
                    repository.UpdateModelStatus(modelId, 2);
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
            IEnumerable<FuzzyVariable> allVariables = repository.GetVariablesForModel(GetCurrentModelId());
            return View("AddFunctionsForVariables", allVariables);
        }

        [Authorize]
        public ActionResult DisplayMembershipFuncDetails(int currentVarId)
        {
            ViewBag.CurrentPage = "create";
            AddVariableIdToSession(currentVarId);
            FuzzyVariable currentVariable = repository.GetVariableById(currentVarId);
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
                return View("AddMembershipFuncDetails", currentVariable.MembershipFunctions.AsEnumerable());
            }
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddMembershipFuncDetails(List<MembershipFunction> listOfMfs)
        {
            ViewBag.CurrentPage = "create";
            FuzzyVariable currentVariable = repository.GetVariableById(GetCurrentVariableId());
            if (ValidateMemmbershipFunctions(new List<FuzzyVariable>(){currentVariable}))
            {
                //validate the range
                listOfMfs = repository.AddMembFuncForVariable(GetCurrentVariableId(), listOfMfs);
                return RedirectToAction("AddFunctionsForVariables");
            }
            else
            {
                ViewBag.VariableMinValue = currentVariable.MinValue;
                ViewBag.VariableMaxValue = currentVariable.MaxValue;
                ModelState.AddModelError("", Resources.Resources.IncorrectMemmbershipFunctionValues);
                return View("AddMembershipFuncDetails", listOfMfs);

            }
        }


        [Authorize] //update
        public ActionResult AddRulesToModel()
        {
            ViewBag.CurrentPage = "create";
            FuzzyModel fuzzyModel = repository.GetModelById(GetCurrentModelId());

            if (DoesEveryVariableHasAllFunctions(fuzzyModel))
            {
                repository.UpdateModelStatus(fuzzyModel, 3);
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
                ModelState.AddModelError("", Resources.Resources.MissingMembershipFunctions);
                return View("AddFunctionsForVariables", fuzzyModel.FuzzyVariables);
            }
        }

        private bool DoesEveryVariableHasAllFunctions(FuzzyModel fuzzyModel)
        {
            int moveForward = 0;
            //czy sa obecne wszystkie zmienne
            foreach (FuzzyVariable variable in fuzzyModel.FuzzyVariables)
            {
                if (variable.NumberOfMembFunc == variable.MembershipFunctions.Count)
                {
                    moveForward++;
                }
            }

            return moveForward == fuzzyModel.FuzzyVariables.Count;
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
                FuzzyModel fuzzyModel = repository.GetModelById(GetCurrentModelId());
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
                repository.AddRulesToModel(fuzzyModel.ModelID, rules);
                repository.UpdateModelStatus(fuzzyModel, 4);
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
            repository.DeleteModelById(modelID);
            return RedirectToAction("BrowseModels", "CreateModel");
        }

        [Authorize]
        public ActionResult ModelDetails(int modelId)
        {
            ViewBag.CurrentPage = "browse";
            ViewBag.IsEdit = false;
            FuzzyModel model = repository.GetModelById(modelId);
            return View("ModelDetails", model);
        }

        [Authorize]
        public ActionResult EditModel(int modelId)
        {
            ViewBag.CurrentPage = "browse";
            FuzzyModel modelObj = repository.GetModelById(modelId);
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
                repository.SaveEditedModel(model);
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
            repository.CopyGivenModel(modelId, GetUserCookieValue());
            return RedirectToAction("BrowseModels");
        }

    }
}
