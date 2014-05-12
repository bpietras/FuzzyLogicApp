using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class MembershipFunction
    {
        [Required(ErrorMessage = "Podaj nazwę funckji przynależności")]
        [Display(Name = "Nazwa:")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Wybierz rodzaj funckji przynależności")]
        [Display(Name = "Rodzaj:")]
        [UIHint("MembershipFunction")]
        public String Type { get; set; } //może zawierać tylko wartości trimf,trampf,gaussmf - może enum?

        //[Required(ErrorMessage = "Punkty muszą być podane w kolejności rosnącej")]
        //[Display(Name = "Ważniejsze punkty:")]
        [HiddenInput(DisplayValue = false)]
        public List<int> ListOfCusps { get; set; } // powinny byc ułożone w kolejnosci rosnącej
    }
}