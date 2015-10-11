
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
                    logger.Debug(String.Format("Początek obliczeń dla modelu: {0}", currentModel.ModelID));
                    Double result = calculator.CalculateTheOutput(currentModel, inputValues);
                    FuzzyVariable outputVariable = currentModel.FuzzyVariables.First(v => v.VariableType == FuzzyLogicService.OutputVariable);
                    logger.Info(String.Format("Wynik dla obliczeń z modelu: {0} wynosi: {1}={2}", currentModel.ModelID, outputVariable.Name, result));
                    return View(new OutputValue(outputVariable.VariableID, Math.Round(result, 2, MidpointRounding.AwayFromZero), outputVariable.Name, inputValues, currentModel.ModelID));
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    HandleErrorInfo errinfo = new HandleErrorInfo(e, "ModelCalculations", "CalculateOutput");
                    return View("Error", errinfo);
                }
            }
            else
            {
                logger.Error(Resources.Resources.OutOfRangeValues);
                ModelState.AddModelError("", Resources.Resources.OutOfRangeValues);
                return View("ModelCalculation", inputValues);
            }
        }

        
    }
}

