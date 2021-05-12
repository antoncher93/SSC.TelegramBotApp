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
    public class MemberUnbannedUpdateHandler : UpdateHandler
    {
        public override void Handle(TelegramBotClient client, Update update)
        {
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.ChatMember &&
                update.ChatMember.NewChatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Member
                && update.ChatMember.OldChatMember.Status != ChatMemberStatus.Kicked)
            {
                var user = update.ChatMember.NewChatMember.User;
                client.SendTextMessageAsync(update.ChatMember.Chat.Id, $"{user.GetMension()} теперь может писать сообщения", Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
            base.Handle(client, update);
        }
    }
}