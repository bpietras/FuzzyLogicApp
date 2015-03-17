using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyLogicWebService.Models.ViewModels
{
    public class VariableValue
    {
        public String VariableName { get; set; }
        public String FunctionName { get; set; }
        public String Connection { get; set; }

        public VariableValue(string variableName, string connection, string functionName)
        {
            VariableName = variableName;
            FunctionName = functionName;
            Connection = connection;
        }
    }
}