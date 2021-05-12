using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public abstract class MessageHandler
    {
        private MessageHandler _next;

        public MessageHandler SetNext(MessageHandler messageHandler)
        {
            _next = messageHandler;
            return messageHandler;
        }

        public virtual void Handle(TelegramBotClient client, Message msg)
        {
            _next?.Handle(client, msg);
        }
    }
}