using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("OutputVariables")]
    public class OVariable
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariableID { get; set; }

        [Required(ErrorMessage = "Podaj nazwę zmiennej")]
        [Display(Name = "Nazwa zmiennej:")]
        public String Name { get; set; }

        [Display(Name = "Typ funkcji przynależności:")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Podaj minimalną wartość zmiennej ")]
        [Display(Name = "Minimalna wartość zmiennej:")]
        public double MinValue { get; set; }

        [Required(ErrorMessage = "Podaj maksymalną wartość zmiennej ")]
        [Display(Name = "Maksymalna wartość zmiennej:")]
        public double MaxValue { get; set; }

        [Required(ErrorMessage = "Podaj liczbę funkcji przynależności")]
        [Display(Name = "Liczba funkcji przynależności:")]
        public int NumberOfMembFunc { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public int ModelID { get; set; }

        [ForeignKey("ModelID")]
        public virtual FuzzyModel FuzzyModel { get; set; }
    }
}