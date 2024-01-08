﻿using PallyWad.Application;

namespace Pallwad.Accounting.Extensions
{
    public static class AWSConfig
    {
        public static void AddAmazonSecretsManager(this IConfigurationBuilder configurationBuilder,
                        string region,
                        string secretName)
        {
            var configurationSource =
                    new AmazonSecretsManagerConfigurationSource(region, secretName);

            configurationBuilder.Add(configurationSource);
        }
    }
}
