﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace FuzzyLogicWebService.FISFiles.FISModel
{
    public class FISSystem
    {
        [Required(ErrorMessage="Podaj nazwę modelu")]
        [Display(Name="Nazwa systemu:")]
        public String Name { get; set; }
        [Required(ErrorMessage="Podaj liczbe wejść")]
        [Range(1,10,ErrorMessage="Liczba wejść musi mieścić się w przedziale 1-10")]
        [Display(Name = "Liczba wejść:")]
        public int InputsNumber { get; set; }
        [Required(ErrorMessage = "Podaj liczbe wyjść")]
        [Range(1, 10, ErrorMessage = "Liczba wyjść musi mieścić się w przedziale 1-10")]
        [Display(Name = "Liczba wyjść:")]
        public int OutputsNumber { get; set; }
        [Required(ErrorMessage="Podaj liczbę reguł")]
        [Range(1, 100,ErrorMessage="Liczba reguł musi mieścić się w przedziale 1-100")]
        [Display(Name = "Liczba reguł:")]
        public int RulesNumber { get; set; }
        
        [HiddenInput(DisplayValue=false)]
        public String AndMethod { get { return "min"; } }
        [HiddenInput(DisplayValue = false)]
        public String OrMethod { get { return "max"; } }
        [HiddenInput(DisplayValue = false)]
        public String ImpMethod { get { return "min"; } }
        [HiddenInput(DisplayValue = false)]
        public String AggMethod { get { return "max"; } }
        [HiddenInput(DisplayValue = false)]
        public String DefuzzMethod { get; set; }
        

    }
}