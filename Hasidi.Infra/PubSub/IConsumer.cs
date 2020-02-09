using System;
using System.Collections.Generic;
using System.Text;

namespace Hasidi.Infra.PubSub
{
    public interface IConsumer<in T, in TM>
    {
        void RegisterHandler(T target, TM handler);
        void Consume();
    }
}
