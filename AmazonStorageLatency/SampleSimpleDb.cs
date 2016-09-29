namespace GettingStartedGuide
{
    using System;
    using System.Collections.Generic;

    using Amazon;
    using Amazon.Runtime.Internal;
    using Amazon.SimpleDB;
    using Amazon.SimpleDB.Model;

    using AmazonStorageLatency;

    class SampleSimpleDb
    {
        public static void Run()
        {
            AmazonSimpleDBClient client = new AmazonSimpleDBClient(RegionEndpoint.USWest2);
            var domainName = "my-Test-Domain";
            var x = "SimpleDB\\CreateDomain".ExecAws(() => client.CreateDomain(new CreateDomainRequest(domainName)));
            Console.WriteLine("CreateDomain response status is " + x.HttpStatusCode);

            AutoConstructedList<ReplaceableItem> items = new AutoConstructedList<ReplaceableItem>();
            for (int i = 1; i <= 5; i++)
            {
                var attrs = new List<ReplaceableAttribute>();
                attrs.Add(new ReplaceableAttribute("Weight", "555", true));
                attrs.Add(new ReplaceableAttribute("Color", "green", true));
                items.Add(new ReplaceableItem("id-" + i + "-" + Guid.NewGuid(), attrs));
            }

            var req1 = new BatchPutAttributesRequest(domainName, items);
            "SimpleDB\\BatchPutAttributes".ExecAws(() => client.BatchPutAttributes(req1));
        }
    }
}