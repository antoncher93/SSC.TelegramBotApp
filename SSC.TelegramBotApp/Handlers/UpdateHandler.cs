using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class UpdateHandler
    {
        private UpdateHandler _next;

        public UpdateHandler SetNext(UpdateHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual void Handle(TelegramBotClient client, Update update)
        {
            _next?.Handle(client, update);
        }
    }
}