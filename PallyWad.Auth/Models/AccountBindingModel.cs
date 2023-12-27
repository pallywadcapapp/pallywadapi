using System.ComponentModel.DataAnnotations;

namespace PallyWad.Auth.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        //[EmailAddress, Phone]
        [Display(Name = "Email")]
        public string Email { get; set; }
        //public string? username { get; set; }
    }
}
