namespace AmazonStorageLatency
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Text;

    using AmazonStorageLatency;

    using GettingStartedGuide;

    class Program
    {
        public static void Main(string[] args)
        {


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

                SampleEC2.Run();
                // continue;
                SampleS3.Run(); 
                SampleSimpleDb.Run();
                SampleDynamoDB.Run();
                SampleIdentity.Run();
                SampleRedis.Run();
            }

        }

    }
}