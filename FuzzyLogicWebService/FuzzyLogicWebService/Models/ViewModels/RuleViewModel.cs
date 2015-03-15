using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.Models.ViewModels
{
    public class RuleViewModel
    {
        public List<VariableValue> InputsValues { get; set; }
        public List<VariableValue> OutputsValues { get; set; }

        public RuleViewModel()
        {
            InputsValues = new List<VariableValue>();
            OutputsValues = new List<VariableValue>();
        }

        public void AddVariable(string varName, string memName, int variableType)
        {
            if (variableType == 0)
            {
                AddInputVariable(varName, memName);
            }
            else
            {
                AddOutputVariable(varName, memName);
            }

        }

        private void AddInputVariable(string varName, string memName)
        {
            InputsValues.Add(new VariableValue(varName, memName));
        }

        private void AddOutputVariable(string varName, string memName)
        {
            OutputsValues.Add(new VariableValue(varName, memName));
        }
    }
}