using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FondoXYZ.Entities.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Email { get; set; }
            = string.Empty;

        [Required]
        public string Token { get; set; }
            = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
            = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
            = string.Empty;
    }
}



