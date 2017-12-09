using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Raven.Utils;
using static System.Net.NetworkInformation.NetworkInterface;

namespace raven
{
    class Program
    {
        static void Main(string[] args)
        {
            var iptarget = new Network().ResolveDNStoIPList("www.google.com")[0];
            Console.WriteLine(iptarget);
            var pingIPs = new Network().Ping_IPs(new[]{iptarget});

            foreach (var p in pingIPs)
            {
                Console.WriteLine("ip {0} ping {1}",p.Key, p.Value);
            }
        }
    }
}
