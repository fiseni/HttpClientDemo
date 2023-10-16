using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConfigureServicePoint();

            while (true)
            {
                Console.Write("Choose option: ");
                var option = Console.ReadLine();

                var response = GetResponseAsync(option).Result;

                Console.WriteLine($"{Environment.NewLine}Response: {response}");

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
                default: return "Not valid option!";
            }
        }

        public static void PrintServicePointInfo()
        {
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(ApiSettings.Instance.ApiUrl));
            var newLine = Environment.NewLine;
            Console.WriteLine(newLine +
                $"ServicePoint Values: {newLine}" +
                $"DefaultConnectionLimit: {ServicePointManager.DefaultConnectionLimit}{newLine}" +
                $"DnsRefreshTimeout: {ServicePointManager.DnsRefreshTimeout}{newLine}" +
                $"EnableDnsRoundRobin: {ServicePointManager.EnableDnsRoundRobin}{newLine}" +
                $"Address: {servicePoint.Address}{newLine}" +
                $"ConnectionLeaseTimeout: {servicePoint.ConnectionLeaseTimeout}{newLine}" +
                $"ConnectionLimit: {servicePoint.ConnectionLimit}{newLine}" +
                $"ConnectionName: {servicePoint.ConnectionName}{newLine}" +
                $"CurrentConnections: {servicePoint.CurrentConnections}{newLine}" +
                $"Expect100Continue: {servicePoint.Expect100Continue}{newLine}" +
                $"IdleSince: {servicePoint.IdleSince}{newLine}" +
                $"MaxIdleTime: {servicePoint.MaxIdleTime}{newLine}" +
                $"ProtocolVersion: {servicePoint.ProtocolVersion}{newLine}" +
                $"SupportsPipelining: {servicePoint.SupportsPipelining}{newLine}" +
                $"------------------------------------------{newLine}");
        }

        public static void ConfigureServicePoint()
        {
            // The servicepoint connection limit is set to 2 by default.
            // But, if the remote host is localhost, the limit is set to Int32.maxvalue (which is just a placeholder for no limit).
            // Therefore we'll explicitly set the limit to 2.
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(ApiSettings.Instance.ApiUrl));
            servicePoint.ConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;
        }
    }
}
