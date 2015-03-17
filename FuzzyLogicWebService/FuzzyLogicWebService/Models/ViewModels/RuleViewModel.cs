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

        public void AddVariable(string varName, string connection, string memName, int variableType)
        {
            if (variableType == 0)
            {
                AddInputVariable(varName, connection, memName);
            }
            else
            {
                AddOutputVariable(varName, connection, memName);
            }

        }

        private void AddInputVariable(string varName, string connection, string memName)
        {
            InputsValues.Add(new VariableValue(varName, connection, memName));
        }

        private void AddOutputVariable(string varName, string connection, string memName)
        {
            OutputsValues.Add(new VariableValue(varName, connection, memName));
        }
    }
}