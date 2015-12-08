using System;
using System.ServiceModel;
using WcfTwitterStreamingServiceLibrary;

namespace WcfTwitterStreamingServiceHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ServiceHost(typeof (WcfService));
            service.Open();

            Console.WriteLine("Running TwitterStreamingService...");
            Console.WriteLine();
            Console.WriteLine("Press any key to stop");
            Console.ReadLine();
        }
    }
}