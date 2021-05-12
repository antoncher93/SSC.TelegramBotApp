using SSC.TelegramBotApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SSC.TelegramBotApp.Handlers
{
    public class ChatMemberBannedHandler : UpdateHandler
    {
        public override void Handle(TelegramBotClient client, Update update)
        {
            base.Handle(client, update);

            if(update.Type == UpdateType.ChatMember 
                    && update.ChatMember.NewChatMember.Status == ChatMemberStatus.Restricted)
            {
                client.SendTextMessageAsync(update.ChatMember.Chat.Id, $"{update.ChatMember.NewChatMember.User.GetMension()} забанен!", ParseMode.Markdown);
            }
        }
    }
}