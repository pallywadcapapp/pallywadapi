namespace PallyWad.Auth.Models
{
    public class UserInfoViewModel
    {
        public string Email { get; set; }
        public string type { get; set; }
        public string SSN { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }
}
