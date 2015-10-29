using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace FuzzyLogicWebService.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "UserNameErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres mailowy")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "UserPasswordErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "UserPassword", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "UserPasswordErrorMsg", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Potwierdzenie hasła")]
        public string ConfirmPassword { get; set; }
    }
}
