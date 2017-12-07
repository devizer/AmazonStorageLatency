using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    class MyAwsPofile
    {
        public static String Name
        {
            get
            {
                var awsProfileName = Environment.GetEnvironmentVariable("AWSProfile");
                if (string.IsNullOrEmpty(awsProfileName))
                {
                    awsProfileName = ConfigurationManager.AppSettings["AWSProfile"];
                }

                if (string.IsNullOrEmpty(awsProfileName))
                {
                    // throw new ConfigurationErrorsException("AWS Profile is not defined via neither config or environment");
                    return "default";
                }

                return awsProfileName;
            }
        }
    }
}
