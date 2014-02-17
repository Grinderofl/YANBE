using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YANBE.Models
{
    public class AccountModels
    {
        public class LoginModel
        {
            [Required]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public bool RememberMe { get; set; }

        }

        public class ChangeSettingsModel
        {
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }
            [DataType(DataType.Password)]
            [Compare("NewPassword")]
            public string ConfirmPassword { get; set; }
            public string Email { get; set; }
        }

        public class RegisterModel
        {
            [Required]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}