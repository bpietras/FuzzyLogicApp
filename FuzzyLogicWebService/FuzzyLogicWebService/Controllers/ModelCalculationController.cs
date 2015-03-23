
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;
using System.IO;
using FuzzyLogicWebService.Models.Functions;

namespace FuzzyLogicWebService.Controllers
{
    public class ModelCalculationController : HigherController
    {

        FuzzyCalculator calculator = new FuzzyCalculator();

        private ModelsRepository rep = new ModelsRepository();

        [Authorize]
        public ActionResult ModelCalculation(int? modelId)
        {
            ViewBag.CurrentPage = "calculations";
            if (modelId != null)
            {
                FuzzyModel currentModel = rep.GetModelById(modelId);
                AddModelIdToSession((int) modelId);
                List<InputValue> inputValues = new List<InputValue>();
                InputValue value = null;
                foreach (FuzzyVariable inputVariable in currentModel.FuzzyVariables)
                {
                    if (inputVariable.VariableType == 0)
                    {
                        value = new InputValue(inputVariable.VariableID, inputVariable.Name);
                        inputValues.Add(value);
                    }
                }
                return View(inputValues);
            }
            else
            {
                return RedirectToAction("BrowseModels", "CreateModel");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CalculateOutput(List<InputValue> inputValues)
        {
            ViewBag.CurrentPage = "calculations";
            try
            {
                FuzzyModel currentModel = rep.GetModelById(GetCurrentModelId());
                double result = calculator.CalculateTheOutput(currentModel, inputValues);
                FuzzyVariable outputVariable = currentModel.FuzzyVariables.Where(v => v.VariableType == 1).First();
                return View(new OutputValue(outputVariable.VariableID, Math.Round(result, 2, MidpointRounding.AwayFromZero)));
            }
            catch (Exception e)
            {
                HandleErrorInfo errinfo = new HandleErrorInfo(e, "ModelCalculations", "CalculateOutput");
                return View("Error", errinfo);
            }
        }

        
    }
}

