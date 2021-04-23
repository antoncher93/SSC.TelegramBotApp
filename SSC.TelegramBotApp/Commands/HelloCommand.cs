using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Commands
{
    public class HelloCommand : Command
    {
        public override string Name => "Привет";

        public override async void Execute(TelegramBotClient client, Message msg)
        {
            var chatId = msg.Chat.Id;
            await client.SendTextMessageAsync(chatId, "Привет:)");
        }
    }
}