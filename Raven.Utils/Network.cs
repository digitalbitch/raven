using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

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


        public IPAddress[] ResolveDNStoIPList(string host)
        {
            var entries = Dns.GetHostEntry(host);
            if (entries.AddressList.Length == 0)
            {
                throw new NetworkInformationException(404);
            }

            return entries.AddressList;
        }

        public Dictionary<string, long> Ping_IPs(IPAddress[] iPs)
        {
            var results = new Dictionary<string, long>();
            foreach (var address in iPs)
            {
                var ping = new Ping();
                var reply = ping.Send(address);
                results.Add(address.ToString(), reply.RoundtripTime);
            }
            return results;
        }

        public Dictionary<string,long> Ping_IPs(List<UnicastIPAddressInformation> iPs)
        {
            var addresses=new IPAddress[iPs.Count];
            var i = 0;
            foreach (var unicastIpAddressInformation in iPs)
            {
               addresses[i] = unicastIpAddressInformation.Address.MapToIPv4();
                i++;
            }
            return Ping_IPs(iPs);
        }
    }
}
