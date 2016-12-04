using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    using System.Diagnostics;
    using System.IO;

    class SampleEC2
    {
        public static void Run()
        {
            DumpSelfMetadata();
        }

        private static void DumpSelfMetadata()
        {
            var f = DumpExtentions.GetLogFileName("EC2\\SelfMetadata");
            using (FileStream fs = new FileStream(f, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter wr = new StreamWriter(fs, Encoding.UTF8))
            {

                if (!AmazonInfo.IsAmazonOnWindows)
                {
                    wr.WriteLine("Not an Amazon Instance on Windows");
                    return;
                }

                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    StringBuilder dump;
                    AmazonInfo.DumpEC2Metadata(0, out dump);
                    wr.WriteLine("Success in " + sw.Elapsed);
                    wr.WriteLine(dump);
                }
                catch (Exception ex)
                {
                    wr.WriteLine("Failed in " + sw.Elapsed + " msec: " + Environment.NewLine + ex);
                }
            }
        }
    }
}
