using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SSC.TelegramBotApp.Handlers
{
    public class AdminMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            var member = client.GetChatMemberAsync(msg.Chat.Id, msg.From.Id).Result;

            if(member.Status == ChatMemberStatus.Administrator 
                || member.Status == ChatMemberStatus.Creator)
            {
                base.Handle(client, msg);
            }
        }
    }
}