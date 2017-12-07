using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    using System.CodeDom;
    using System.Collections;
    using System.Reflection;
    using System.ServiceProcess;

    using Amazon.EC2.Util;

    using Universe;

    // http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/ec2-instance-metadata.html
    // http://docs.aws.amazon.com/sdkfornet/v3/apidocs/Index.html
    class AmazonInfo
    {
        public static bool DumpEC2Metadata(int intent, out StringBuilder dump)
        {
            dump = new StringBuilder();
            string prefix = new string(' ', Math.Max(0,intent));
            var properties = typeof (EC2Metadata).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var p in properties)
            {
                string key = p.Name;
                Exception err = null;
                object value = null;

                try
                {
                    value = p.GetValue(null);
                }
                catch (Exception ex)
                {
                    err = ex;
                }
                dump.Append(prefix + key + ": ");
                if (err != null)
                    dump.Append(err.GetType().Name + ": " + err.Message);
                else if (value == null)
                    dump.Append("<< null >>");
                else if (value is IEnumerable && !(value is string))
                {
                    dump.AppendLine();
                    foreach (var item in (IEnumerable) value)
                    {
                        dump.AppendLine(prefix + "   " + item);
                        if (item != null && item.GetType().Name == "NetworkInterface")
                        {
                            string json = JSonExtentions.ToNewtonJSon(item, true);
                            dump.AppendLine(json);
                        }
                    }
        
                }
                else
                    dump.Append(value);

                if (value != null && value.GetType().Name == "NetworkInterface")
                {
                    string json = JSonExtentions.ToNewtonJSon(value, true);
                    dump.AppendLine(json);
                }

                dump.AppendLine();
            }

            return true;
        }
        
        public static bool IsAmazonOnWindows { get { return _IsAmazonOnWindows.Value; }}

        private static Lazy<bool> _IsAmazonOnWindows = new Lazy<bool>(() =>
        {
            try
            {
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                    return false;

                ServiceController sc = new ServiceController("Ec2Config");
                var status = sc.Status;
                return true;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }

            return false;
        });
    }

}
