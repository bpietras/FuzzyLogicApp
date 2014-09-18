using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;

namespace FuzzyLogicWebService.Controllers
{
    public class ModelCalculationController : HigherController
    {
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
            CalculateTheOutput(currentModel, inputValues);
            return View();
        }

        private void CalculateTheOutput(FuzzyModel model, List<InputValue> inputValues)
        {
            
        }
    }
}
