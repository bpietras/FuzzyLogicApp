using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("FuzzyVariables")]
    public class FVariable
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariableID { get; set; }

        [Required(ErrorMessage = "Podaj nazwę zmiennej")]
        [Display(Name = "Nazwa zmiennej:")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Podaj minimalną wartość zmiennej ")]
        [Display(Name = "Minimalna wartość zmiennej:")]
        public Double MinValue { get; set; }

        [Required(ErrorMessage = "Podaj maksymalną wartość zmiennej ")]
        [Display(Name = "Maksymalna wartość zmiennej:")]
        public Double MaxValue { get; set; }

        [Required(ErrorMessage = "Podaj liczbę funkcji przynależności")]
        [Display(Name = "Liczba funkcji przynależności:")]
        public int NumberOfMembFunc { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ModelID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int VariableType { get; set; } //0 for input, 1 for output

        [ForeignKey("ModelID")]
        public virtual FuzzyModel FuzzyModel { get; set; }

        public IEnumerable<MembershipFunction> MfFunctions { get; set; }

    }
}