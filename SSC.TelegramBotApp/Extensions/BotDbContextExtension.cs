using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Extensions
{
    public static class BotDbContextExtension
    {
        public static UserInfo GetUserInfo(this BotDbContext botDB, User user)
        {
            try
            {
                var userInfo = botDB.UserInfoes.FirstOrDefault(ui => ui.TelegramId.Equals(user.Id));
                if (userInfo is null)
                {
                    userInfo = new UserInfo()
                    {
                        TelegramId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.Username
                    };
                    botDB.UserInfoes.Add(userInfo);
                    botDB.Entry(userInfo).State = System.Data.Entity.EntityState.Added;
                    botDB.SaveChanges();
                }
                return userInfo;
            }
            catch(Exception e)
            {
                throw;
            }
        }


        public static Member GetMember(this BotDbContext botDB, long chatId, long userId)
        {
            try
            {
                var member = botDB.Members.FirstOrDefault(m => m.ChatId.Equals(chatId) && m.UserId.Equals(userId));
                if (member is null)
                {
                    member = new Member()
                    {
                        ChatId = chatId,
                        UserId = userId,
                        Warns = 0
                    };
                    member = botDB.Members.Add(member);
                    botDB.Entry(member).State = System.Data.Entity.EntityState.Added;
                    botDB.SaveChanges();
                }
                return member;
            }
            catch(Exception e)
            {
                throw;
            }
          
        }
    }
}