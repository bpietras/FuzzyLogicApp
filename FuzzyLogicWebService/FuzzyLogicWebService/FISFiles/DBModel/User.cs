using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("Users")]
    public class User
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessageResourceName = "UserNameErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "UserPasswordErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "UserPassword", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public virtual ICollection<FuzzyModel> Models { get; set; }
    }
}