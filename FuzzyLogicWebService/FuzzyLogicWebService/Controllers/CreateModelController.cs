﻿using System;
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
        ModelsRepository rep = new ModelsRepository();
        
        [Authorize]
        public ActionResult Create(int? modelId)
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
            return View(rep.GetUserModels(id));
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
                fuzzyModel = rep.AddModelForUser(id,fuzzyModel);
                AddModelIdToSession(fuzzyModel.ModelID);
                return RedirectToAction("AddInputVariables", new { modelId = fuzzyModel.ModelID });
            }
            else
            {
                return RedirectToAction("Create", fuzzyModel.ModelID);
            }
            
        }


        [Authorize]
        public ActionResult AddInputVariables(int? modelId)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            FuzzyModel fuzzyModel = rep.GetModelById(modelId);
            List<FuzzyVariable> inputs = new List<FuzzyVariable>();
            for (int w = 0; w < fuzzyModel.InputsNumber; w++)
            {
                inputs.Add(FuzzyLogicModel.FuzzyVariable.CreateFuzzyVariable(0));
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
                FuzzyModel fuzzyModel = rep.GetModelById(GetCurrentModelId());
                rep.AddInputVariableForModel(fuzzyModel.ModelID, listOfInVariables);
                return RedirectToActionPermanent("AddOutputVariables", new { modelId = fuzzyModel.ModelID });
            }
            else
            {
                return View("AddInputVariables", listOfInVariables);
            }
        }

        [Authorize]
        public ActionResult AddOutputVariables(int? modelId)
        {
            ViewBag.CurrentPage = "create";
            ViewBag.IsEdit = false;
            FuzzyModel fuzzyModel = rep.GetModelById(modelId);
            List<FuzzyVariable> outputs = new List<FuzzyVariable>();
            for (int w = 0; w < fuzzyModel.OutputsNumber; w++)
            {
                outputs.Add(FuzzyLogicModel.FuzzyVariable.CreateFuzzyVariable(1));
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
                int modelId = GetCurrentModelId();
                rep.AddOutputVariableForModel(modelId, listOfOutVariables);
                IEnumerable<FuzzyVariable> allVariables = rep.GetVariablesForModel(modelId);
                return View("AddFunctionsForVariables", allVariables);
            }
            else
            {
                return View("AddOutputVariables", listOfOutVariables);
            }
        }

        [Authorize]
        public ActionResult DisplayMembershipFuncDetails(int currentVarId)
        {
            ViewBag.CurrentPage = "create";
           
            //Session["currentVariableId"] = currentVarId;
            AddVariableIdToSession(currentVarId);
            FuzzyVariable currentVariable = rep.GetVariableById(currentVarId);
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
        public ActionResult AddMembershipFuncDetails(IEnumerable<MembershipFunction> listOfMfs)
        {
            ViewBag.CurrentPage = "create";
            if (ModelState.IsValid)
            {
                listOfMfs = rep.AddMembFuncForVariable(GetCurrentVariableId(), listOfMfs);
                IEnumerable<FuzzyVariable> allVariables = rep.GetVariablesForModel(GetCurrentModelId());
                return View("AddFunctionsForVariables", allVariables);
            }
            else
            {
                return View("AddMembershipFuncDetails", listOfMfs);

            }
        }


        [Authorize]
        public ActionResult AddRulesToModel()
        {
            ViewBag.CurrentPage = "create";
            FuzzyModel fuzzyModel = rep.GetModelById(GetCurrentModelId());
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
                int modelID = GetCurrentModelId();
                rep.AddRulesToModel(modelID, rules);
                return RedirectToAction("ModelDetails", new { modelId = modelID });
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
            ViewBag.IsEdit = false;
            FuzzyModel model = rep.GetModelById(modelId);
            return View("ModelDetails", model);
        }

        [Authorize]
        public ActionResult EditModel(int? modelId)
        {
            ViewBag.CurrentPage = "browse";
            FuzzyModel modelObj = rep.GetModelById(modelId);
            ViewBag.VariableWidth = (int)100/modelObj.FuzzyVariables.Count;
            return View("EditModel", modelObj);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditModel(FuzzyModel model)
        {
            ViewBag.CurrentPage = "browse";
            FuzzyModel modelObj = rep.EditModel(model);
            return View("ModelDetails", modelObj);
        }
    }
}
