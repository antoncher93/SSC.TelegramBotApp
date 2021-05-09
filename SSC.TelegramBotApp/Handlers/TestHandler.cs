﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class TestHandler : BaseHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if (msg.Text.IndexOf("!Test", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var chatId = msg.Chat.Id;
                client.SendTextMessageAsync(chatId, "Test Success!", replyToMessageId: msg.MessageId);
            }
            else base.Handle(client, msg);
        }
    }
}