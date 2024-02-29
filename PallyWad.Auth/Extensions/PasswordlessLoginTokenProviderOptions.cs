using Microsoft.AspNetCore.Identity;

namespace PallyWad.Auth.Extensions
{
    public class PasswordlessLoginTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public PasswordlessLoginTokenProviderOptions()
        {
            // update the defaults
            Name = "PasswordlessLoginTokenProvider";
            TokenLifespan = TimeSpan.FromMinutes(1440);
        }
    }
}
