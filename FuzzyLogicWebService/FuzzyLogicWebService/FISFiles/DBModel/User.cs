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

        [Required(ErrorMessage="No name given")]
        [Display(Name = "User name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "No password given")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        public virtual ICollection<FuzzyModel> Models { get; set; }
    }
}