using System;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfTwitterStreamingConsoleClient.TwitterStreamingServiceReference;

namespace WcfTwitterStreamingConsoleClient
{
    public class Program
    {
        public static void Main(string[] filters)
        {
            var callbackContext = new InstanceContext(new WcfServiceCallback());
            var client = new WcfServiceClient(callbackContext, "NetHttpBinding_IWcfService");

            client.GetStreaming(filters);

            Console.ReadKey();
        }
    }

    public class WcfServiceCallback : IWcfServiceCallback
    {
        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void StreamStarted()
        {
        }

        public void StreamPaused()
        {
        }

        public void StreamResumed()
        {
        }

        public void StreamStopped()
        {
        }

        public Task PrintMessageAsync(string message)
        {
            return Task.Factory.StartNew
                (
                    () => PrintMessage(message)
                );
        }
    }
}