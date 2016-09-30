namespace AmazonStorageLatency
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;

    using AmazonStorageLatency;

    using GettingStartedGuide;

    class Program
    {
        public static void Main(string[] args)
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            {
                Console.WriteLine("AWSProfileName was not set in the App.config file.");
                return;
            }

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("ITERATION " + i);
                try
                {
                    Directory.Delete("Logs", true);
                }
                catch (Exception)
                {
                }
                
                SampleS3.Run(); 
                SampleSimpleDb.Run();
                SampleDynamoDB.Run();
                SampleIdentity.Run();
                SampleRedis.Run();
            }

        }

    }
}