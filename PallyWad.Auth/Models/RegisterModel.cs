using System.ComponentModel.DataAnnotations;

namespace PallyWad.Auth.Models
{
    public class RegisterModel
    {
        /*[Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }*/

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [Phone]
        public string? phoneNo { get; set; }
        public string? fullname { get; set; }
        public string SSN { get; set; }
        public string type { get; set; }
    }

    public enum UserType
    {
        user,
        provider,
        admin
    }

    public class UserPersonalInfoModel{
        public string userId { get; set; }
        public string? contactaddress { get; set; }
        public DateTime DOB { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public string countryofresidene { get; set; }

    }

}
