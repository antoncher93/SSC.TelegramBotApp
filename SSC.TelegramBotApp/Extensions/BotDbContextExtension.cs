using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSC.TelegramBotApp.Extensions
{
    public static class BotDbContextExtension
    {
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