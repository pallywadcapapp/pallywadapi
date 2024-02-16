using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class AWSAppConfiguration : IAWSAppConfiguration
    {
        // Keep the following details in appsettings.config file or DB or Enivironment variable
        // Get those values from it and assign to the below varibales. Based on the approach , modify the below code.
        public AWSAppConfiguration()
        {
            BucketName = "";
            Region = "";
            AwsAccessKey = "";
            AwsSecretAccessKey = "";
            AwsSessionToken = "";
        }

        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string AwsSessionToken { get; set; }
    }

    public interface IAWSAppConfiguration
    {
        string AwsAccessKey { get; set; }
        string AwsSecretAccessKey { get; set; }
        string AwsSessionToken { get; set; }
        string BucketName { get; set; }
        string Region { get; set; }
    }
}
