using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    using System.IO;
    using System.Threading;

    class DumpExtentions
    {
        private static int Counter = 0;

        public static string GetLogFileName(string actionName)
        {
            Interlocked.Increment(ref Counter);

            string ret = Path.Combine("Logs", actionName);
            ret = Path.Combine(
                Path.GetDirectoryName(ret),
                Counter.ToString("0000") + "-" + Path.GetFileName(ret)
                );

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ret));
            }
            catch
            {
            }

            return ret;
        }


    }
}
