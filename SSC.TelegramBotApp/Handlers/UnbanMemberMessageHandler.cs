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
    public class UnbanMemberMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Text != null && msg.Text.IndexOf("/unban") >= 0)
            {
                if (msg.Entities != null)
                {
                    int i = 0;
                    foreach(var entity in msg.Entities)
                    {
                        var user = entity.User;
                        if (user != null)
                            user.Unban(client, msg.Chat.Id);
                        else
                        {
                            var username = msg.EntityValues.ElementAt(i).Replace("@", "");
                            var userInfo = BotDbContext.Get().UserInfoes.FirstOrDefault(u => u.Username.Equals(username));
                            if (userInfo != null)
                            {
                                var chatMember = client.GetChatMemberAsync(msg.Chat.Id, userInfo.TelegramId).Result;
                                chatMember.User.Unban(client, msg.Chat.Id);
                            }
                        }
                        i++;
                    }
                }
                else
                    client.SendTextMessageAsync(msg.Chat.Id, "Для разблокировки необходимо указать пользователей!", replyToMessageId: msg.MessageId);

            }
            else base.Handle(client, msg);
        }
    }
}