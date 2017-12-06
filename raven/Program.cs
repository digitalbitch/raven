using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var network = new Network();
                var localIPs = network.Network_IPs();
                Console.WriteLine("getting local address info");
                foreach (var unicastIpAddressInformation in localIPs)
                {
                    Console.WriteLine(unicastIpAddressInformation.Address.ToString());
                }
                Console.WriteLine("pinging");
                var pingResults = network.Ping_IPs(localIPs);
                foreach (var pingResult in pingResults)
                {
                    Console.WriteLine(pingResult.Key, pingResult.Value);
                }
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                e = null;
            }
           
        }
    }
}
