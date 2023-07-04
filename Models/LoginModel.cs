using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rental_Car.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter email address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email_address { get; set; }


        [Required(ErrorMessage = "Please enter password.")]
        public string user_password { get; set; }
    }
}