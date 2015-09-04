using System;
using System.Windows;
using System.Windows.Controls;
using ZappChat_v3.Core.Messaging;

namespace ZappChat_v3.Controls.TemplateSelector
{
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncomingTextMessageDataTemplate { get; set; }
        public DataTemplate OutgoingTextMessageDataTemplate { get; set; }
        public DataTemplate IncomingStickerMessageDataTemplate { get; set; }
        public DataTemplate OutgoingStickerMessageDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var message = item as Message;
            if (message == null)
                throw new NullReferenceException(
                    "Ошибка при установке шаблона Message! Ссылка на несуществующий объект.");
            switch (message.Class)
            {
                case MessageClass.Text:
                    return message.Type == MessageType.Incomming
                        ? IncomingTextMessageDataTemplate
                        : OutgoingTextMessageDataTemplate;
                case MessageClass.Sticker:
                    return message.Type == MessageType.Incomming
                        ? IncomingStickerMessageDataTemplate
                        : OutgoingStickerMessageDataTemplate;
                default:
                    return null;
            }
        }
    }
}