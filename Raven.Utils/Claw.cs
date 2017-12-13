using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace Raven.Utils
{
    public class Claw
    {
        public List<string> Sniff(int lowerPort,int upperPort,IPAddress ip)
        {
            List<string> results=new List<string>();
            Parallel.For(lowerPort, upperPort, async (int i) =>
            {
                TcpClient scanner=new TcpClient();
                try
                {
                    scanner.Connect(ip, i);
                    results.Add($"port {i} open on {ip.ToString()}");
                }
                catch (Exception ex)
                {
                    results.Add($"port {i} unavailable on {ip.ToString()}");
                }

               ;
                await Task.Delay(10);
            });
            return results;
        }
    }
}