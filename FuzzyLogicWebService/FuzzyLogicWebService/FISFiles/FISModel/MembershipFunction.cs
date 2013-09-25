using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class MembershipFunction
    {
        public String Name { get; set; }
        public String Type { get; set; } //może zawierać tylko wartości trimf,trampf,gaussmf - może enum?
        public List<int> ListOfCusps { get; set; } // powinny byc ułożone w kolejnosci rosnącej
    }
}