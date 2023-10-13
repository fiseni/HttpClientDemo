using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Choose option: ");
                var option = Console.ReadLine();

                var response = GetResponseAsync(option).Result;

                Console.WriteLine(response);

                PrintServicePointInfo();
            }
        }

        public static async Task<string> GetResponseAsync(string option)
        {
            switch (option)
            {
                case "1": return await new ClientNetFX1.Service1().GetStatusCodeAsync().ConfigureAwait(false);
                case "11": return await new ClientNetFX1.Service11().GetStatusCodeAsync().ConfigureAwait(false);
                case "2": return await new ClientNetFX2.Service2().GetStatusCodeAsync().ConfigureAwait(false);
                case "21": return await new ClientNetFX2.Service21().GetStatusCodeAsync().ConfigureAwait(false);
                case "3": return await new ClientNetFX3.Service3().GetStatusCodeAsync().ConfigureAwait(false);
                case "4": return await new ClientNetFX4.Service4().GetStatusCodeAsync().ConfigureAwait(false);
                case "5": return await new ClientNetFX5.Service5().GetStatusCodeAsync().ConfigureAwait(false);
                case "6": return await new ClientNetFX6.Service6().GetStatusCodeAsync().ConfigureAwait(false);
                case "61": return await new ClientNetFX6.Service61().GetStatusCodeAsync().ConfigureAwait(false);
                case "62": return await new ClientNetFX6.Service62().GetStatusCodeAsync().ConfigureAwait(false);
                case "63": return new ClientNetFX6.Service63().GetStatusCode();
                case "64": return new ClientNetFX6.Service64().GetStatusCode();
                default: return "Not valid option!";
            }
        }

        public static void PrintServicePointInfo()
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

        public static void ConfigureServicePoint()
        {
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(ApiSettings.Instance.ApiUrl));
            servicePoint.ConnectionLeaseTimeout = 5 * 1000;
            servicePoint.ConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;
        }
    }
}
