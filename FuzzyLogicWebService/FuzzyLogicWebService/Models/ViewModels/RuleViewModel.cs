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
        
    }
}