using Microsoft.AspNetCore.Identity;

namespace PallyWad.Auth.Extensions.Provider
{
    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddPasswordlessLoginTotpTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var totpProvider = typeof(PasswordlessLoginTotpTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("PasswordlessLoginTotpProvider", totpProvider);
        }

        public static IdentityBuilder AddPasswordlessLoginTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var provider = typeof(PasswordlessLoginTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("PasswordlessLoginProvider", provider);
        }
    }
}
