using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Extensions
{
    public static class UserExtensions
    {
        public static string GetMension(this User member)
        {
            return "[" + member.FirstName + "](tg://user?id=" + member.Id + ")";
        }

        public static void Unban(this User user, TelegramBotClient client, long chatId)
        {
            var permissions = new ChatPermissions()
            {
                CanAddWebPagePreviews = true,
                CanChangeInfo = true,
                CanInviteUsers = true,
                CanPinMessages = true,
                CanSendMediaMessages = true,
                CanSendMessages = true,
                CanSendOtherMessages = true,
                CanSendPolls = true
            };

            client.RestrictChatMemberAsync(chatId, user.Id, permissions);
        }

        public static void BanInChat(this User user, TelegramBotClient client, long chatId, int replyMessageId = 0,
            DateTime untilDate = default)
        {
            var permissions = new ChatPermissions()
            {
                CanSendMediaMessages = false,
                CanSendMessages = false,
                CanSendOtherMessages = false
            };

            client.RestrictChatMemberAsync(chatId, user.Id, permissions, untilDate);

            if(replyMessageId > 0)
                client.SendTextMessageAsync(chatId, $"{user.GetMension()} будет забанен!", 
                    replyToMessageId: replyMessageId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

            var member = BotDbContext.Get().GetMember(chatId, user.Id);
            member.Warns = 0;
            BotDbContext.Get().Entry(member).State = System.Data.Entity.EntityState.Modified;
            BotDbContext.Get().SaveChanges();
        }
    }
}