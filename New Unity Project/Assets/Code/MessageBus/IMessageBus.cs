using System;

namespace TankGame.Messaging
{
public interface IMessageBus  {

        void Publish<TMessage>( TMessage message )
            where TMessage : IMessage;

        ISubscription<TMessage>
            Subscribe<TMessage>( Action<TMessage> action )
            where TMessage : IMessage;

        void UnSubscribe<TMessage>( ISubscription<TMessage> subscription )
            where TMessage : IMessage;

        void Clear();
}
}
