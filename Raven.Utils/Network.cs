using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Raven.Utils
{
    public class Network
    {
        private readonly Dictionary<string, string> _whoisServerDictionary;

        public Network(Dictionary<string,string> whoisServerDictionary)
        {
            _whoisServerDictionary = whoisServerDictionary;
        }

        public Network()
        {
        }

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
        //credit : http://dotnet-snippets.com/snippet/whois-lookup/668
        //ref: https://de.wikipedia.org/wiki/Whois#Liste_von_Whois-Servern
        //ref: http://www.nirsoft.net/whois_servers_list.html
        public string GetWhoisInformation(string whoisServer, string url)
        {
            StringBuilder stringBuilderResult = new StringBuilder();
            TcpClient tcpClinetWhois = new TcpClient(whoisServer, 43);
            NetworkStream networkStreamWhois = tcpClinetWhois.GetStream();
            BufferedStream bufferedStreamWhois = new BufferedStream(networkStreamWhois);
            StreamWriter streamWriter = new StreamWriter(bufferedStreamWhois);

            streamWriter.WriteLine(url);
            streamWriter.Flush();

            StreamReader streamReaderReceive = new StreamReader(bufferedStreamWhois);

            while (!streamReaderReceive.EndOfStream)
                stringBuilderResult.AppendLine(streamReaderReceive.ReadLine());

            return stringBuilderResult.ToString();
        }

        public string GetWhoisInformation(string url )
        {
            if (_whoisServerDictionary == null)
            {
                throw new KeyNotFoundException("whois server dictionary not set in constructor");
            }
          
            var s = string.Empty;
            var parts = url.ToLowerInvariant().Split('.').Reverse().ToArray();
            if (_whoisServerDictionary.Any(kv => kv.Key == $"{parts[1]}.{parts[0]}"))
            {
                s = _whoisServerDictionary[$"{parts[1]}.{parts[0]}"];
            }
            else
            {
                if (_whoisServerDictionary.Any(kv => kv.Key == parts[0]))
                {
                    s = _whoisServerDictionary[parts[0]];
                }
                else
                {
                    throw new KeyNotFoundException("Domain extension not found in dictionary of whois servers");
                }
            }
            return GetWhoisInformation(s, url);
        }

    }
}
