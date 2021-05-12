using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class AgreementAcceptUpdateHandler : UpdateHandler
    {
        public override void Handle(TelegramBotClient client, Update update)
        {
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                && update.CallbackQuery.Data.Equals(Bot.ACCEPT_AGREEMENT_CALLBACK))
            {
                var user = update.CallbackQuery.From;
                user.Unban(client, update.CallbackQuery.Message.Chat.Id);
            }
            else base.Handle(client, update);
        }
    }
}