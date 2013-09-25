using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class Rule
    {
        public List<int> inputs { get; set; }
        public List<int> outputs { get; set; }
        public String weight = " (1)";
        public int connection { get; set; }
    }
}