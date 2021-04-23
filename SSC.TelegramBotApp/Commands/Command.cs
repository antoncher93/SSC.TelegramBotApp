using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract void Execute(TelegramBotClient client, Message msg);
        public bool Contains(string command)
        {
            return command.Contains(Name);
        }
    }
}