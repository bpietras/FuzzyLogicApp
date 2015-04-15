using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.Models
{
    public class InputValue
    {
        public int VariableId { get; set; }
        public Double VariableValue { get; set; }
        public string VariableName { get; set; }
        public Double VariableMinValue { get; set; }
        public Double VariableMaxValue { get; set; }

        public InputValue(int variableId, string variableName,
            Double minValue, Double maxValue)
        {
            this.VariableId = variableId;
            this.VariableName = variableName;
            this.VariableMinValue = minValue;
            this.VariableMaxValue = maxValue;
        }

        public InputValue()
        {
        }
    }
}