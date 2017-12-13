using Raven.Utils;
using System;
using System.Linq;
using System.Net;

namespace raven
{
    class Program
    {
        static void Main(string[] args)
        {
            var results=new Claw().Sniff(442, 444, new Network().ResolveDNStoIPList("www.google.com")[0]);



            var iptarget = new Network().ResolveDNStoIPList("www.google.com")[0];
            Console.WriteLine(iptarget);
            var pingIPs = new Network().Ping_IPs(new[]{iptarget});

            foreach (var p in pingIPs)
            {
                Console.WriteLine("ip {0} ping {1}",p.Key, p.Value);
            }

            var whoisdata = new Network().GetWhoisInformation("whois.verisign-grs.com", "google.com");
            Console.Write(whoisdata);

            var whoisServerDictionary =new Data().ExtractWhoisServerList("whoisServers.dat");

            foreach (var entry in whoisServerDictionary)
            {
                string formatedOutput=String.Format("|{0,10}|{1,10}",entry.Key, entry.Value);
                Console.WriteLine(formatedOutput);
            }
             var targetWhoIs1= new Network(whoisServerDictionary).GetWhoisInformation("google.co.uk");
             var targetWhoIs2 = new Network(whoisServerDictionary).GetWhoisInformation("ed.ac.uk");

        }
    }
}
