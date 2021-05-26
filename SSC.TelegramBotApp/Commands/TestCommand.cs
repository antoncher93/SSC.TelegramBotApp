using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SSC.TelegramBotApp.Commands
{
    public class TestCommand : AdminCommand
    {
        public override string Name => "/test";

        public override void Execute(TelegramBotClient client, Message msg)
        {
            client.SendTextMessageAsync(msg.Chat, "Test Success!", replyToMessageId: msg.MessageId);
           
          
        }
    }
}