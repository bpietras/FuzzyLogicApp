using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISSystem
    {
        public String Name { get; set; }
        public int InputsNumber { get; set; }
        public int OutputsNumber { get; set; }
        public int RulesNumber { get; set; }
        
        public String AndMethod { get { return "min"; } }
        public String OrMethod { get { return "max"; } }
        public String ImpMethod { get { return "min"; } }
        public String AggMethod { get { return "max"; } }
        public String DefuzzMethod { get { return "centroid"; } }
        
    }
}