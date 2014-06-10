using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("Models")]
    public class FuzzyModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModelID { get; set; }

        [Required(ErrorMessageResourceName = "SystemNameErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "SystemName", ResourceType=typeof(Resources.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "DescriptionErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Description", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "InputsNumberErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "InputsNumber", ResourceType = typeof(Resources.Resources))]
        [Range(1, 10, ErrorMessage = "Liczba wejść musi mieścić się w przedziale 1-10")]
        public int InputsNumber { get; set; }

        [Required(ErrorMessageResourceName = "OutputsNumberErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "OutputsNumber", ResourceType = typeof(Resources.Resources))]
        [Range(1, 10, ErrorMessage = "Liczba wyjść musi mieścić się w przedziale 1-10")]
        public int OutputsNumber { get; set; }

        [Required(ErrorMessageResourceName = "RulesNumberErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "RulesNumber", ResourceType = typeof(Resources.Resources))]
        [Range(1, 100, ErrorMessage = "Liczba reguł musi mieścić się w przedziale 1-100")]
        public int RulesNumber { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual IEnumerable<FVariable> Variables { get; set; }
        public virtual IEnumerable<Rule> Rules { get; set; }


    }
}