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

        public static void BanInChat(this User user, TelegramBotClient client, long chatId, int replyMessageId = 0, DateTime untilDate = default)
        {
            var permissions = new ChatPermissions()
            {
                //CanAddWebPagePreviews = true,
                //CanChangeInfo = true,
                //CanInviteUsers = true,
                //CanPinMessages = true,
                CanSendMediaMessages = false,
                CanSendMessages = false,
                CanSendOtherMessages = false
                //CanSendPolls = true
            };

            client.RestrictChatMemberAsync(chatId, user.Id, permissions, untilDate);

            if(replyMessageId > 0)
                client.SendTextMessageAsync(chatId, $"{user.GetMension()} будет забанен!", 
                    replyToMessageId: replyMessageId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }

        public static void Warn(this User member, TelegramBotClient client, long chatId, long msgId)
        {
            var warn = 0;
            var userId = member.Id;
            var botDB = BotDbContext.Get();
            var user = botDB.UserInfoes.FirstOrDefault(u => u.TelegramId.Equals(userId));
            if (user is null)
            {
                user = new Models.UserInfo()
                {
                    TelegramId = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    IsBanned = true,
                    Warns = 0
                };

                botDB.UserInfoes.Add(user);
                botDB.Entry(user).State = System.Data.Entity.EntityState.Added;
                botDB.SaveChangesAsync().Wait();
            }

            user.Warns++;
            warn = user.Warns;
            botDB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            botDB.SaveChangesAsync();


            client.SendTextMessageAsync(chatId, member.GetMension() + " Предупреждение номер " + warn);
        }
    }
}