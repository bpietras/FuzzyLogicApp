
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;
using System.IO;
using FuzzyLogicWebService.Models.Functions;
using FuzzyLogicWebService.Helpers;
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService.Controllers
{
    public class ModelCalculationController : HigherController
    {
        public ModelCalculationController(IDatabaseRepository modelRepository, ILogger appLogger)
            : base(modelRepository, appLogger)
        {
        }

        FuzzyCalculator calculator = new FuzzyCalculator();

        [Authorize]
        public ActionResult ModelCalculation(int modelId = 0)
        {
            ViewBag.CurrentPage = "calculations";
            if (modelId != 0)
            {
                FuzzyModel currentModel = repository.GetModelById(modelId);
                AddModelIdToSession((int) modelId);
                List<InputValue> inputValues = new List<InputValue>();
                InputValue value = null;
                foreach (FuzzyVariable inputVariable in currentModel.FuzzyVariables)
                {
                    if (inputVariable.VariableType == FuzzyLogicService.InputVariable)
                    {
                        value = new InputValue(inputVariable.VariableID, inputVariable.Name, inputVariable.MinValue, inputVariable.MaxValue);
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
            if (ValidateInputValues(inputValues))
            {
                try
                {
                    FuzzyModel currentModel = repository.GetModelById(GetCurrentModelId());
                    Double result = calculator.CalculateTheOutput(currentModel, inputValues);
                    FuzzyVariable outputVariable = currentModel.FuzzyVariables.First(v => v.VariableType == FuzzyLogicService.OutputVariable);
                    return View(new OutputValue(outputVariable.VariableID, Math.Round(result, 2, MidpointRounding.AwayFromZero), outputVariable.Name, inputValues, currentModel.ModelID));
                }
                catch (Exception e)
                {
                    HandleErrorInfo errinfo = new HandleErrorInfo(e, "ModelCalculations", "CalculateOutput");
                    return View("Error", errinfo);
                }
            }
            else
            {
                ModelState.AddModelError("", "Podane wartości nie należą do odpowiednich zakresów");
                return View("ModelCalculation", inputValues);
            }
        }

        
    }
}

