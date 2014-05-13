using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required(ErrorMessage="No name given")]
        public string Name { get; set; }
        [Required(ErrorMessage = "No password given")]
        public string Password { get; set; }
        
    }
}