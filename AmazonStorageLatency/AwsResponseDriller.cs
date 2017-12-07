using System;
using System.Text;

namespace AmazonStorageLatency
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Universe;

    public static class AwsResponseDriller
    {
        public static TResponse ExecAws<TResponse>(this string actionName, Func<TResponse> func)
        {
            Stopwatch sw = Stopwatch.StartNew();
            TResponse ret;
            DateTime startAt = DateTime.Now;
            Exception err = null;
            try
            {
                sw = Stopwatch.StartNew();
                ret = func();
            }
            catch (Exception ex)
            {
                err = ex;
                ret = default(TResponse);
            }
            long msec = sw.ElapsedMilliseconds;
            DateTime finishedAt = DateTime.Now;


            var f = DumpExtentions.GetLogFileName(actionName);
            using (FileStream fs = new FileStream(f + ".log", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter wr = new StreamWriter(fs, Encoding.UTF8))
            {
                wr.WriteLine("Started at {0}", startAt);
                if (err == null)
                {
                    string msg = string.Format("Action {0} is successful in {1} msec.", actionName, msec);
                    wr.WriteLine(msg + " Response is:");
                    wr.WriteLine(ret.ToNewtonJSon(true));
                    Console.WriteLine(msg);
                }
                else
                {
                    string msg = string.Format("Action {0} FAILED in {1} msec.", actionName, msec);
                    wr.WriteLine(msg + " Exception is: " + err.Message);
                    wr.WriteLine(err);
                    Console.WriteLine(msg + " Exception is: " + err.Message);
                }

                wr.WriteLine("Finished at {0}", finishedAt);

            }

            return ret;


        }
    }
}
