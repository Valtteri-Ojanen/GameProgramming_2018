using System;

namespace TankGame.Messaging
{
    public interface ISubscription<TMessage> : IDisposable
        where TMessage : IMessage
    {
        Action<TMessage> Action { get; }

        IMessageBus MessageBus { get; }
    }
}
