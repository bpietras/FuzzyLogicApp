using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISSystem
    {
        public String Name { get; set; }
        public int InputsNumber { get; set; }
        public int OutputsNumber { get; set; }
        public int RulesNumber { get; set; }
        public String AndMethod { get; set; }
        public String OrMethod { get; set; }
        public String ImpMethod { get; set; }
        public String AggMethod { get; set; }
        public String DefuzzMethod { get; set; }
    }
}