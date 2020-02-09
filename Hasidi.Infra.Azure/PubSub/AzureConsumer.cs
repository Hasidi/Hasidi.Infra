using System;
using System.Threading;
using System.Threading.Tasks;
using Hasidi.Infra.ObjectConvertors;
using Hasidi.Infra.PubSub;
using Microsoft.Azure.ServiceBus;

namespace Hasidi.Infra.Azure.PubSub
{
    public class AzureConsumer<T> : IConsumer<string, AzureConsumerHandlerData<T>>
    {
        private readonly string _connectionUrl;
        private readonly IObjectConvertor _objectConvertor;
        private QueueClient _queueClient;
        private MessageHandlerOptions _messageHandlerOptions;
        private Func<Message, CancellationToken, Task> _handler;
        private Func<T, Task> _inputHandler;

        public AzureConsumer(string connectionUrl, IObjectConvertor objectConvertor)
        {
            _connectionUrl = connectionUrl;
            _objectConvertor = objectConvertor;
        }


        public void RegisterHandler(string target, AzureConsumerHandlerData<T> handlerData)
        {
            if (handlerData.Handler == null || handlerData.ExceptionHandler == null)
                throw new ArgumentException("Please supply all handler data params");
            _queueClient = new QueueClient(_connectionUrl, target);
            _messageHandlerOptions = new MessageHandlerOptions(handlerData.ExceptionHandler)
            {
                MaxConcurrentCalls = handlerData.NConsumers,
                AutoComplete = false
            };
            _inputHandler = handlerData.Handler;
            _handler = HandleMessage;
        }

        public void Consume()
        {
            if (_handler == null || _messageHandlerOptions == null)
                throw new ArgumentException("Please Register Handler first");
            _queueClient.RegisterMessageHandler(_handler, _messageHandlerOptions);
        }

        private async Task HandleMessage(Message message, CancellationToken token)
        {
            try
            {
                var obj = _objectConvertor.ConvertToObject<T>(message.Body);
                await _inputHandler(obj);
            }
            finally
            {
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }
    }
}
