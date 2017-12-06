using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Utils
{
    public class Network
    {
        public List<UnicastIPAddressInformation> Network_IPs()
        {
            var results=new List<UnicastIPAddressInformation>();

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                results.AddRange(ni.GetIPProperties().UnicastAddresses.Where(ip => !ip.IsDnsEligible).Where(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            }

            return results;
        }

        public Dictionary<string,long> Ping_IPs(List<UnicastIPAddressInformation> iPs)
        {
            var results=new Dictionary<string, long>();
            foreach (var unicastIpAddressInformation in iPs)
            {
                try
                {
                    var address = unicastIpAddressInformation.Address.MapToIPv4();
                    Ping ping = new Ping();
                    var reply = ping.Send(address.ToString());
                    results.Add(address.ToString(), reply.RoundtripTime);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    e = null;
                }
              
              
            }
            return results;
        }
    }
}
