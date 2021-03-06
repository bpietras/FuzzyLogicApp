﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("Rules")]
    public class Rule
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleID { get; set; }

        public int ModelID { get; set; }

        [Required(ErrorMessageResourceName = "RuleContentErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "RuleContent", ResourceType = typeof(Resources.Resources))]
        public string RuleContent { get; set; }

        [ForeignKey("ModelID")]
        public virtual FuzzyModel FuzzyModel { get; set; }
    }
}