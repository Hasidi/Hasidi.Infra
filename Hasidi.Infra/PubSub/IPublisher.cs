using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hasidi.Infra.PubSub
{
    public interface IPublisher<in Q, in T> : IDisposable
    {
        void Init(Q target);
        Task PublishAsync(T message);
    }
}
