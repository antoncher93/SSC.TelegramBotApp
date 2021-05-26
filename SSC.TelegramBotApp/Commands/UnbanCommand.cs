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
    public class UnbanCommand : MemberBotCommand
    {
        public UnbanCommand(Action<TelegramBotClient, Chat, User, int> execute) : base("/unban", execute)
        {

        }
    }
}