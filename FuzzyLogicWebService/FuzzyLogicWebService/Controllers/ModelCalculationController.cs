using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;
using System.IO;
using FuzzyLogicWebService.Views.ModelCalculation;

namespace FuzzyLogicWebService.Controllers
{
    public class ModelCalculationController : HigherController
    {

        FuzzyCalculator calculator = new FuzzyCalculator();

        private ModelsRepository rep = new ModelsRepository();

        [Authorize]
        public ActionResult ModelCalculation(int modelId)
        {
            FuzzyModel currentModel = rep.GetModelById(modelId);
            AddModelIdToSession(modelId);
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

        [Authorize]
        [HttpPost]
        public ActionResult CalculateOutput(List<InputValue> inputValues)
        {
            FuzzyModel currentModel = rep.GetModelById(GetCurrentModelId());
            double result = calculator.CalculateTheOutput(currentModel, inputValues);

            return View();
        }

        
    }
}
