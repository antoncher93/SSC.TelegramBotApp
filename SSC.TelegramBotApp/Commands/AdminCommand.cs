using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Commands
{
    public abstract class AdminCommand
    {
        public abstract string Name { get; }
        public abstract void Execute(TelegramBotClient client, Message msg);
        public async Task ExecuteAsync(TelegramBotClient client, Message msg)
        {
            await Task.Factory.StartNew(() => Execute(client, msg));
        }
    }
}