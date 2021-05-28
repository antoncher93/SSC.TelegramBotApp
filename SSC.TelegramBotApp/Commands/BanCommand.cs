using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SSC.TelegramBotApp.Commands
{
    public class BanCommand : MemberBotCommand
    {
        public BanCommand(Action<TelegramBotClient, Chat, User, int, DateTime> execute) 
            : base("/ban", (client, chat, user, msgId) => _Ban(execute, client, chat, user, msgId))
        {
        }

        private static void _Ban(Action<TelegramBotClient, Chat, User, int, DateTime> execute, TelegramBotClient client, Chat chat, User user, int replyToMsgId)
        {
            execute?.Invoke(client, chat, user, replyToMsgId, DateTime.UtcNow + TimeSpan.FromDays(3));
        }
    }
}