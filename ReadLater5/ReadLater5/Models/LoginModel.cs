using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadLater5.Models
{
    public class LoginModel
    {
        [Required]
        [BindProperty]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public bool WrongCredentials { get; set; }
    }
}
