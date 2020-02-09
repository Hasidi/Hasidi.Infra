using System.Threading.Tasks;
using Hasidi.Infra.ObjectConvertors;
using Hasidi.Infra.PubSub;
using Microsoft.Azure.ServiceBus;

namespace Hasidi.Infra.Azure.PubSub
{
    public class AzurePublisher<T> : IPublisher<string, T>
    {
        private readonly string _connectionUrl;
        private readonly IObjectConvertor _objectConvertor;
        private QueueClient _queueClient;

        public AzurePublisher(string connectionUrl, IObjectConvertor objectConvertor)
        {
            _connectionUrl = connectionUrl;
            _objectConvertor = objectConvertor;
        }

        public void Init(string queueName)
        {
            _queueClient = new QueueClient(_connectionUrl, queueName);
        }

        public async Task PublishAsync(T message)
        {
            var bytes = _objectConvertor.ConvertToBytes(message);
            var msg = new Message(bytes);
            await _queueClient.SendAsync(msg);
        }

        public void Dispose()
        {
            var task = _queueClient.CloseAsync();
            task.Wait();
        }
    }
}
