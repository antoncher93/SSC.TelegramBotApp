using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class BaseHandler
    {
        private BaseHandler _next;

        public BaseHandler SetNext(BaseHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual void Handle(TelegramBotClient client, Message msg)
        {
            _next?.Handle(client, msg);
        }
    }
}