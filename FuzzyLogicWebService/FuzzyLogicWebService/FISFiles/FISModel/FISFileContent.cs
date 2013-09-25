using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISFileContent
    {
        public FISSystem SystemProperties { get; set; }
        public List<FISVariable> InputVariables { get; set; }
        public List<FISVariable> OutputVariables { get; set; }
        public List<Rule> ListOfRules { get; set; }
    }
}