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

        [Required(ErrorMessageResourceName = "FVariableNameErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "FVariableName", ResourceType = typeof(Resources.Resources))]
        public String Name { get; set; }

        [Required(ErrorMessageResourceName = "MinValueErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "MinValue", ResourceType = typeof(Resources.Resources))]
        public Double MinValue { get; set; }

        [Required(ErrorMessageResourceName = "MaxValueErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "MaxValue", ResourceType = typeof(Resources.Resources))]
        public Double MaxValue { get; set; }

        [Required(ErrorMessageResourceName = "NumberOfMembFuncErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "NumberOfMembFunc", ResourceType = typeof(Resources.Resources))]
        public int NumberOfMembFunc { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ModelID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int VariableType { get; set; } //0 for input, 1 for output

        [ForeignKey("ModelID")]
        public virtual FuzzyModel FuzzyModel { get; set; }

        public virtual IEnumerable<MembershipFunction> MfFunctions { get; set; }

    }
}