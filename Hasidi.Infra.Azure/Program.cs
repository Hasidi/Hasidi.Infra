using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Hasidi.Infra.Azure.PubSub;
using Hasidi.Infra.ObjectConvertors;
using Microsoft.Azure.ServiceBus;

namespace Hasidi.Infra.Azure
{
    class Program
    {
        public static string ServiceBusConnectionString = "Endpoint=sb://hassampleservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YY6QDk6Rlmk8jkocrJ4Px0hWBZmxYr71zbRCJi+WeXE=";
        public static string QueueName = "stringqueue";

        static void Main(string[] args)
        {

            //_ = RunPublisher();
            _ = RunConsumer();

            Console.Read();
        }

        private static async Task RunPublisher()
        {
            var p = new AzurePublisher<string>(ServiceBusConnectionString, new JsonObjectConvertor());
            p.Init(QueueName);
            while (true)
            {
                await p.PublishAsync("Hello world");
                Console.Out.WriteLine("----a message was published");
                Thread.Sleep(20000);
            }
        }

        private static async Task RunConsumer()
        {
            var c = new AzureConsumer<string>(ServiceBusConnectionString, new JsonObjectConvertor());
            c.RegisterHandler(QueueName, handlerData: new AzureConsumerHandlerData<string>()
            {
                ExceptionHandler = eventArgs => Task.CompletedTask,
                Handler = ReceiveMessagesAsync,
                NConsumers = 4
            });
            c.Consume();
        }

        private static async Task ReceiveMessagesAsync(string message)
        {
            Console.WriteLine("About to execute consuming");
            await Task.Delay(100);
            Console.WriteLine("finish consuming message is: " + message);
            //await Task.Delay(20000);
        }
    }
}
