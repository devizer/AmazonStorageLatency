using System;
using System.Collections.Generic;

namespace AmazonStorageLatency
{
    using System.Diagnostics;
    using System.Threading;

    using Amazon;
    using Amazon.DynamoDBv2;
    using Amazon.DynamoDBv2.Model;

    using Universe;

    class SampleDynamoDB
    {
        public static void Run()
        {
            Stopwatch sw;
            TimeSpan elapsed;
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(RegionEndpoint.USWest2);

            var t2Response = "DynamoDB\\DescribeTable(Table2)".ExecAws(() => client.DescribeTable("Table2"));

            var noSuchTable = "DynamoDB\\DescribeTable(noSuchTable)".ExecAws(() => client.DescribeTable("Table3sdfsdgsfg"));

            var listTablesResponse = "DynamoDB\\ListTables".ExecAws(() => client.ListTables());

            var req = new BatchWriteItemRequest();
            req.ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL;
            req.ReturnItemCollectionMetrics = ReturnItemCollectionMetrics.SIZE;
            PutRequest put1 = new PutRequest(new Dictionary<string, AttributeValue>()
            {
                { "Logon", new AttributeValue("hehe") },
                { "Any_Number", new AttributeValue() { N = "13"}},
                { "Any_String", new AttributeValue() { S = "what the fuck"}},
                { "Any_True", new AttributeValue() { BOOL = true}},
                { "Any_False", new AttributeValue() { BOOL = false}},
                { "Any_StringList", new AttributeValue() { SS = new List<string>() { "three", "two", "one"}}},
            });

            req.RequestItems.GetOrCreateDefault("Table2").Add(new WriteRequest(put1));
            sw = Stopwatch.StartNew();

            var writeBatchResponse = "DynamoDB\\BatchWriteItem".ExecAws(() => client.BatchWriteItem(req));

            var tableName = "PhoenixTable";

            if (listTablesResponse.TableNames.Contains(tableName))
            {
                ("DynamoDB\\DeleteTable(" + tableName + ")").ExecAws(() => client.DeleteTable(tableName));
                int nn = 0;
                Stopwatch swDelete = Stopwatch.StartNew();
                while (true)
                {
                    Thread.Sleep(55);
                    var actionName = ("DynamoDB\\DescribeTable(" + tableName + ")-" + (++nn));
                    var r = actionName.ExecAws(() => client.DescribeTable(tableName));
                    if (r == null) break;
                }
                Console.WriteLine("DynamoDB Table DELETED in {0} msec", swDelete.ElapsedMilliseconds);
            }

            var createTableRequest = new CreateTableRequest(
                tableName,
                new List<KeySchemaElement>()
                {
                    new KeySchemaElement("Id", KeyType.HASH),
                    new KeySchemaElement("CreatedAt", KeyType.RANGE)
                },
                new List<AttributeDefinition>()
                {
                    new AttributeDefinition("Id", ScalarAttributeType.B),
                    new AttributeDefinition("CreatedAt", ScalarAttributeType.S),
                },
                new ProvisionedThroughput(1, 1));

            
                
            var newTableResponse = "DynamoDB\\CreateTable".ExecAws(() => client.CreateTable(createTableRequest));

            // Wait for CREATED
            int n = 1;
            Stopwatch swCreating = Stopwatch.StartNew();
            while (true)
            {
                Thread.Sleep(55);
                string logKey = "DynamoDB\\Wait using DescribeTable-" + (n++);
                var res = logKey.ExecAws(() => client.DescribeTable(tableName));
                if (res != null && res.Table.TableStatus != "CREATING") break;
            }
            Console.WriteLine("DynamoDB " + tableName + " CREATED in " + swCreating.ElapsedMilliseconds + " msecs");
            
            // "DynamoDB\\DeleteTable".ExecAws(() => client.DeleteTable(tableName));
        }
    }

    public static class DictionaryExtentions
    {
        public static TValue GetOrCreateDefault<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key) where TValue : new ()
        {
            TValue value;
            if (!d.TryGetValue(key, out value))
            {
                value = new TValue();
                d[key] = value;
            }

            return value;
        }
    }
}

