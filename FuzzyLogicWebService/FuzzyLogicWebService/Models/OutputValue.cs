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

        public OutputValue(int variableId, double outcome)
        {
            this.VariableId = variableId;
            this.OutputPoint = outcome;
        }

        public OutputValue()
        {
        }
    }
}