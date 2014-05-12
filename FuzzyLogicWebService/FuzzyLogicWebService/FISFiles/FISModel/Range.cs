using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class Range
    {
        [Required(ErrorMessage="Podaj dolną granicę wartości zmiennej")]
        [Display(Name = "Dolna granica wartości zmiennej:")]
        public int MinValue { get; set; }
        [Required(ErrorMessage = "Podaj górną granicę wartości zmiennej")]
        [Display(Name = "Górna granica wartości zmiennej:")]
        public int MaxValue { get; set; } //min<max musi byc zachowane
    }
}