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
    public class BanUserMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg != null && msg.Text != null && msg.Text.IndexOf("/ban", StringComparison.OrdinalIgnoreCase)>=0)
            {
                if(msg.Entities != null)
                {
                    int i = 0;
                    foreach(var entity in msg.Entities)
                    {
                        var user = entity.User;
                        if(user != null)
                        {
                            user.BanInChat(client, msg.Chat.Id, msg.MessageId, DateTime.UtcNow + TimeSpan.FromDays(3));
                        }
                        else
                        {
                            var username = msg.EntityValues.ElementAt(i).Replace("@", "");
                            var userInfo = BotDbContext.Get().UserInfoes.FirstOrDefault(u => u.Username.Equals(username));
                            if (userInfo != null)
                            {
                                var chatMember = client.GetChatMemberAsync(msg.Chat.Id, userInfo.TelegramId).Result;
                                chatMember.User.BanInChat(client, msg.Chat.Id, msg.MessageId, DateTime.UtcNow + TimeSpan.FromDays(3));
                            }
                        }

                        i++;
                    }
                }
                else if(msg.ReplyToMessage != null)
                {
                    msg.ReplyToMessage.From
                        .BanInChat(client, msg.Chat.Id, msg.MessageId, DateTime.UtcNow + TimeSpan.FromDays(3));
                }
            }
            else base.Handle(client, msg);
        }
    }
}