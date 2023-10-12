using System;
using System.Net;

namespace HttpClientDemo.ClientNetFX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Choose option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1": Console.WriteLine(new ClientNetFX1.Service1().GetEntries().Result); break;
                    case "11": Console.WriteLine(new ClientNetFX1.Service11().GetEntries().Result); break;
                    case "2": Console.WriteLine(new ClientNetFX2.Service2().GetEntries().Result); break;
                    case "22": Console.WriteLine(new ClientNetFX2.Service22().GetEntries().Result); break;
                    case "3": Console.WriteLine(new ClientNetFX3.Service3().GetEntries().Result); break;
                    case "4": Console.WriteLine(new ClientNetFX4.Service4().GetEntries().Result); break;
                    case "5": Console.WriteLine(new ClientNetFX5.Service5().GetEntries().Result); break;
                    default: break;
                }

                PrintServicePointInfo();
            }
        }

        private static void PrintServicePointInfo()
        {
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(ApiSettings.Instance.ApiUrl));
            Console.WriteLine($"ServicePointManager: " + Environment.NewLine +
                $"DefaultConnectionLimit: {ServicePointManager.DefaultConnectionLimit} " + Environment.NewLine +
                $"DnsRefreshTimeout: {ServicePointManager.DnsRefreshTimeout} " + Environment.NewLine +
                $"EnableDnsRoundRobin: {ServicePointManager.EnableDnsRoundRobin} " + Environment.NewLine +
                $"Address: {servicePoint.Address} " + Environment.NewLine +
                $"ConnectionLeaseTimeout: {servicePoint.ConnectionLeaseTimeout} " + Environment.NewLine +
                $"ConnectionLimit: {servicePoint.ConnectionLimit} " + Environment.NewLine +
                $"ConnectionName: {servicePoint.ConnectionName} " + Environment.NewLine +
                $"CurrentConnections: {servicePoint.CurrentConnections} " + Environment.NewLine +
                $"Expect100Continue: {servicePoint.Expect100Continue} " + Environment.NewLine +
                $"IdleSince: {servicePoint.IdleSince} " + Environment.NewLine +
                $"MaxIdleTime: {servicePoint.MaxIdleTime} " + Environment.NewLine +
                $"ProtocolVersion: {servicePoint.ProtocolVersion} " + Environment.NewLine +
                $"SupportsPipelining: {servicePoint.SupportsPipelining}");
        }

        private static void ConfigureServicePoint()
        {
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(ApiSettings.Instance.ApiUrl));
            servicePoint.ConnectionLeaseTimeout = 5 * 1000;
            servicePoint.ConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;
        }
    }
}
