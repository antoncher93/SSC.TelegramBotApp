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
        private readonly Action<TelegramBotClient, Chat, User> _unban;

        public AgreementAcceptUpdateHandler(Action<TelegramBotClient, Chat, User> unban)
        {
            _unban = unban;
        }

        public override void Handle(TelegramBotClient client, Update update)
        {
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                && update.CallbackQuery.Data.Equals(Bot.ACCEPT_AGREEMENT_CALLBACK))
            {
                var user = update.CallbackQuery.From;
                _unban(client, update.CallbackQuery.Message.Chat, user);
            }
            else base.Handle(client, update);
        }
    }
}