using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    using System.CodeDom;
    using System.ServiceProcess;

    class AmazonInfo
    {
        public static bool IsAmazonOnWindows { get { return _IsAmazonOnWindows.Value; }}
        
        static Lazy<bool> _IsAmazonOnWindows = new Lazy<bool>(() =>
        {
            try
            {
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
