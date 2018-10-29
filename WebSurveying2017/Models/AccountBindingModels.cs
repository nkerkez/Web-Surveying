using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using WebSurveying2017.Model.Model;
using FluentValidation.Attributes;
using WebSurveying2017.Validation;

namespace WebSurveying2017.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Šifra mora imati minimalno 6 karaktera.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Šifre se ne poklapaju unesite ih opet.")]
        public string ConfirmPassword { get; set; }
    }


    [Validator(typeof(SignUpValidation))]
    public class RegisterBindingModel
    {
        
        public string Email { get; set; }

        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public string RoleName { get; set; }

        public string City { get; set; }
        
        
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Gender Gender { get; set; }

        public DateTime Birthday { get; set; }
    }

    [Validator(typeof(UpdateUserValidation))]
    public class UpdateUserBindingModel
    {

        public string Email { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }

        public string City { get; set; }

        public DateTime? Birthday { get; set; }


    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Šifra mora imati minimalno 6 karaktera.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Šifre se ne poklapaju unesite ih opet.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordBindinfModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Šifra mora imati minimalno 6 karaktera.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Šifre se ne poklapaju unesite ih opet.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
