namespace GettingStartedGuide
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;

    using AmazonS3Sample;

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
            }

        }

    }
}