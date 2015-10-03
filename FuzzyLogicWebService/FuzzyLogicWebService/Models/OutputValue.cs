using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.Models
{
    public class OutputValue
    {
        public int VariableId { get; set; }
        public Double OutputPoint { get; set; }
        public string OutputName { get; set; }
        public List<InputValue> InputValues { get; set; }
        public int ModelID { get; set; }

        public OutputValue(int variableId, Double outcome, string name, List<InputValue> inputValues, int modelId)
        {
            this.VariableId = variableId;
            this.OutputName = name;
            this.OutputPoint = outcome;
            this.InputValues = inputValues;
            this.ModelID = modelId;
        }

        public OutputValue()
        {
        }
    }
}