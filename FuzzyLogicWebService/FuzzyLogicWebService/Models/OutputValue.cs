using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.Models
{
    public class OutputValue
    {
        public int VariableId { get; set; }
        public double OutputPoint { get; set; }
        public string OutputName { get; set; }
        public List<InputValue> InputValues { get; set; }

        public OutputValue(int variableId, double outcome, string name, List<InputValue> inputValues)
        {
            this.VariableId = variableId;
            this.OutputName = name;
            this.OutputPoint = outcome;
            this.InputValues = inputValues;
        }

        public OutputValue()
        {
        }
    }
}