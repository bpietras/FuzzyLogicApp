using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("MembershipFunctions")]
    public class MembershipFunction
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FunctionID { get; set; }

        [Required(ErrorMessage = "Podaj nazwę funkcji przynależności")]
        [Display(Name = "Nazwa:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wybierz rodzaj funkcji przynależności")]
        [Display(Name = "Rodzaj:")]
        //[UIHint("MembershipFunction")]
        public string Type { get; set; } //może zawierać tylko wartości trimf,trampf,gaussmf - może enum?

        public Double MinValue { get; set; }
        public Double? MinAverValue { get; set; }
        public Double? MaxAverValue { get; set; }
        public Double MaxValue { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int VariableID { get; set; }

        [ForeignKey("VariableID")]
        public virtual FVariable FuzzyVariable { get; set; }
    }
}