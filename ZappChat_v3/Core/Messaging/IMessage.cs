using System.ComponentModel;

namespace ZappChat_v3.Core.Messaging
{
    public interface IMessage : INotifyPropertyChanged
    {
        long Id { get; }

        MessageType Type { get; }
    }
}