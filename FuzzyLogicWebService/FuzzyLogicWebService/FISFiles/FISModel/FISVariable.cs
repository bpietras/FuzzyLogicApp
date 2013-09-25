using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISVariable
    {
        public string Type { get; set; }
        public String Name { get; set; }
        public Range RangeOfValues { get; set; }
        public List<MembershipFunction> ListOfMF { get; set; }
    }
}