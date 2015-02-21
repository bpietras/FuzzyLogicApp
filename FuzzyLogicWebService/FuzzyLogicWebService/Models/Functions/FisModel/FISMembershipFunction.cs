using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISMembershipFunction
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public List<Double> ListOfCusps { get; set; }
        public int Index { get; set; }
    }
}