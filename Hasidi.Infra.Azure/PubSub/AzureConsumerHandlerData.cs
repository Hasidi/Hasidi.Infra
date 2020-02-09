using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Hasidi.Infra.Azure.PubSub
{
    public class AzureConsumerHandlerData<T>
    {
        public Func<ExceptionReceivedEventArgs, Task> ExceptionHandler;
        public Func<T, Task> Handler;
        public int NConsumers = 1;
    }
}
