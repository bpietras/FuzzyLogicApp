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

        [Required(ErrorMessageResourceName = "MembershipFunctionNameErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "MembershipFunctionName", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "TypeErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "FunctionType", ResourceType = typeof(Resources.Resources))]
        //[UIHint("MembershipFunction")]
        public string Type { get; set; } //może zawierać tylko wartości trimf,trampf,gaussmf - może enum?

        [Required(ErrorMessageResourceName = "FirstValueErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "FirstValue", ResourceType = typeof(Resources.Resources))]
        public Double FirstValue { get; set; }

        [Required(ErrorMessageResourceName = "SecondValueErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "SecondValue", ResourceType = typeof(Resources.Resources))]
        public Double? SecondValue { get; set; }

        [Required(ErrorMessageResourceName = "ThirdValueErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ThirdValue", ResourceType = typeof(Resources.Resources))]
        public Double? ThirdValue { get; set; }

        [Display(Name = "FourthValue", ResourceType = typeof(Resources.Resources))]
        public Double FourthValue { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int VariableID { get; set; }

        [ForeignKey("VariableID")]
        public virtual FVariable FuzzyVariable { get; set; }
    }
}