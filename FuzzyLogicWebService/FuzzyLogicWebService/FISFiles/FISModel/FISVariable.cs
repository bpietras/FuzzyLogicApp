﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISVariable
    {
        [Required(ErrorMessage = "Podaj nazwę zmiennej")]
        [Display(Name = "Nazwa zmiennej:")]
        public String Name { get; set; }

        [Display(Name = "Typ funkcji przynależności:")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Podaj dolną granicę wartości zmiennej")]
        [Display(Name = "Dolna granica wartości zmiennej:")]
        public int MinValue { get; set; }
        [Required(ErrorMessage = "Podaj górną granicę wartości zmiennej")]
        [Display(Name = "Górna granica wartości zmiennej:")]
        public int MaxValue { get; set; } //min<max musi byc zachowane

        [Required(ErrorMessage = "Podaj liczbę funkcji przynależności")]
        [Display(Name = "Liczba funkcji przynależności:")]
        public int NumberOfMembFunc { get; set; }

        public List<MembershipFunction> ListOfMF { get; set; }

    }
}