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
            var network = new Network();
            network.Network_IPs();
            Console.Read();
        }
    }
}
