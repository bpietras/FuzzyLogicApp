using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.Models
{
    public class InputValue
    {
        public int VariableId { get; set; }
        public Decimal VariableValue { get; set; }
        public string VariableName { get; set; }
        public Decimal VariableMinValue { get; set; }
        public Decimal VariableMaxValue { get; set; }

        public InputValue(int variableId, string variableName,
            Decimal minValue, Decimal maxValue)
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