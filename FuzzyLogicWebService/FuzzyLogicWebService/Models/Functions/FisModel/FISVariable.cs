using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISVariable
    {
       
        public string Name { get; set; }
        public string Type { get; set; }
        public Double MinValue { get; set; }
        public Double MaxValue { get; set; } //todo: min<max musi byc zachowane
        public int NumberOfMembFunc { get; set; }

        public List<FISMembershipFunction> ListOfMF { get; set; }
        public int Index { get; set; }
    }
}