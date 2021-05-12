using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class WarnUserMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg != null && msg.Text != null && msg.Text.IndexOf("!Warn", StringComparison.OrdinalIgnoreCase) >=0)
            {
                if(msg.Entities != null)
                {
                    var botDb = BotDbContext.Get();
                    foreach (var entity in msg.Entities)
                    {
                        var user = entity.User;
                        
                        var userInfo = botDb.UserInfoes.FirstOrDefault(u => u.TelegramId.Equals(user.Id));
                        if (userInfo is null)
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
                        var mention = "[" + user.FirstName + "](tg://user?id=" + user.Id + ")";
                        client.SendTextMessageAsync(msg.Chat.Id, $"{mention}, предупреждение №{warn}!", replyToMessageId: msg.MessageId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                }
            }
            else
            {
                base.Handle(client, msg);
            }
        }
    }
}