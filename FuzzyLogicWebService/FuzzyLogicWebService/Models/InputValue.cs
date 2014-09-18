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

        public InputValue(int variableId, string variableName)
        {
            this.VariableId = variableId;
            this.VariableName = variableName;
        }

        public InputValue()
        {
        }
    }
}