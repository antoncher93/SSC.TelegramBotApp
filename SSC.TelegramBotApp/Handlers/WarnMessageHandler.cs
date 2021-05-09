using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class WarnMessageHandler : BaseHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Text.IndexOf("!warn", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if(msg.ReplyToMessage != null)
                {
                    var user = msg.ReplyToMessage.From;
                    var botDb = BotDbContext.Get();
                    var userInfo = botDb.UserInfoes.FirstOrDefault(u => u.TelegramId.Equals(user.Id));
                    if(userInfo is null)
                    {
                        userInfo = new UserInfo()
                        {
                            TelegramId = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            IsBanned = false,
                            Warns = 0
                        };

                        botDb.UserInfoes.Add(userInfo);
                        botDb.Entry(userInfo).State = System.Data.Entity.EntityState.Added;
                        botDb.SaveChangesAsync();
                    }

                    var warn = userInfo.Warns++;
                    botDb.Entry(userInfo).State = System.Data.Entity.EntityState.Modified;
                    botDb.SaveChangesAsync();
                    var warns = userInfo.Warns;

                    client.SendTextMessageAsync(msg.Chat.Id, $"Предупреждение №{warn}!", replyToMessageId: msg.ReplyToMessage.MessageId);
                }
            }

            base.Handle(client, msg);
        }
    }
}