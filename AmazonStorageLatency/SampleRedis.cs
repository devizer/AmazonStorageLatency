using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    using System.CodeDom;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Amazon.Runtime;

    using StackExchange.Redis;

    class SampleRedis
    {

        private static string ClusterEndpointFormat
        {
            get
            {
                var ret = ConfigurationManager.AppSettings["RedisCluster"];
                return ret;
            }
        }

        private static ConfigurationOptions MasterConfig;
        private static ConfigurationOptions SlaveConfig;
        
        static SampleRedis()
        {
            MasterConfig = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ResolveDns = true,
                EndPoints = { string.Format(ClusterEndpointFormat, 1) },
                ConnectTimeout = 2000
            };

            SlaveConfig = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ResolveDns = true,
                EndPoints = { string.Format(ClusterEndpointFormat, 2) },
                ConnectTimeout = 2000
            };
        }

        public static void Run()
        {
            var log = new StringWriter();
            using (var master = ConnectionMultiplexer.Connect(MasterConfig, log))
            using (var slave = ConnectionMultiplexer.Connect(SlaveConfig, log))

            {
                if (!master.IsConnected)
                {
                    Console.WriteLine("Redis MASTER-connection failed" + Environment.NewLine + log);
                }
                else if (!slave.IsConnected)
                {
                    Console.WriteLine("Redis SLAVE-connection failed" + Environment.NewLine + log);
                }
                else
                {
                    // heat?
                    master.GetDatabase().StringSet(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                    StringBuilder buffer = new StringBuilder();
                    int count = 100;
                    for (int i = 0; i <= count; i++)
                    {
                        string key = Guid.NewGuid().ToString();
                        string expected = Guid.NewGuid().ToString();
                        Stopwatch sw = Stopwatch.StartNew();
                        master.GetDatabase().StringSet(key, expected);
                        decimal msec = 1000m*sw.ElapsedTicks/Stopwatch.Frequency;
                        if (i==0)
                            buffer.AppendLine("Action Redis\\StringSet@Master is successeful in " + msec + " msec");

                        Stopwatch wait = Stopwatch.StartNew();
                        string actual = null;
                        long iterations = 0;
                        while (actual == null && wait.ElapsedMilliseconds < 1000)
                        {
                            actual = (string) slave.GetDatabase().StringGet(key);
                            // if (actual != expected) Thread.Sleep(0);
                            iterations++;
                        }

                        var ticksReplication = wait.ElapsedTicks;
                        decimal msecReplication = 1000m*ticksReplication/Stopwatch.Frequency;
                        if (actual == expected)
                        {
                            buffer.AppendFormat("MASTER-SLAVE replication is succesful in {0} msec, {1} iterations", msecReplication, iterations).AppendLine();
                        }
                        else
                        {
                            buffer.AppendFormat("MASTER-SLAVE replication failed by timeout in {0} msec", msecReplication).AppendLine();
                        }
                        if (i%10 == 0 || i == count)
                        {
                            Console.WriteLine(buffer);
                            buffer = new StringBuilder();
                        }
                    }
                }
            }

        }
    }
}
